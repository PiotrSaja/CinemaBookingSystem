using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using Microsoft.Extensions.Configuration;

namespace CinemaBookingSystem.Api.Services
{
    public class UserVoteService : IUserVoteService
    {
        private int NUM_CLASTERS;
        private readonly IConfiguration _configuration;
        private readonly CinemaDbContext _context;
        private readonly ILogger<UserVoteService> _logger;

        #region UserVoteService()

        public UserVoteService(IConfiguration configuration, CinemaDbContext context, ILogger<UserVoteService> logger)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Clustering()
        public async Task<bool> Clustering(PerformContext context, CancellationToken cancellationToken)
        {
            NUM_CLASTERS = _configuration.GetValue<int>("UserVote:Clusters");

            var timer = new Stopwatch();
            _logger.LogInformation("Start clustering");
            context.WriteLine("Start clustering");

            _logger.LogInformation("Job running at: {time}", DateTimeOffset.UtcNow);
            context.WriteLine($"Job running at: {DateTimeOffset.UtcNow}");

            timer.Start();

            var movies = await _context.Movies.OrderBy(x => x.Id).Select(x=>x.Id).ToListAsync(cancellationToken);

            var usersId = await _context.UserMovieVotes.Where(x => x.StatusId == 1).Select(x => x.UserId).Distinct().ToListAsync(cancellationToken);

            if (usersId.Count < (NUM_CLASTERS * 4))
            {
                _logger.LogInformation("To little users votes for clustering method");
                context.WriteLine("To little users votes for clustering method");

                return false;
            }

            _logger.LogInformation("1 step. Time: {elapsedTime}", timer.Elapsed);
            context.WriteLine($"1 step - select from db. Time: {timer.Elapsed}");

            timer.Restart();

            var rawDataFromDb = await GetRawDataFromDatabase(movies, usersId, cancellationToken);

            int[] result = GetClusters(rawDataFromDb, NUM_CLASTERS);

            var table = new DataTable();
            table.Columns.Add(nameof(UserCluster.UserId), typeof(string));
            table.Columns.Add(nameof(UserCluster.ClusterNumber), typeof(int));
            table.Columns.Add(nameof(UserCluster.CreatedBy), typeof(string));
            table.Columns.Add(nameof(UserCluster.Created), typeof(DateTime));
            table.Columns.Add(nameof(UserCluster.ModifiedBy), typeof(string));
            table.Columns.Add(nameof(UserCluster.Modified), typeof(DateTime));
            table.Columns.Add(nameof(UserCluster.StatusId), typeof(int));
            table.Columns.Add(nameof(UserCluster.InactivatedBy), typeof(string));
            table.Columns.Add(nameof(UserCluster.Inactivated), typeof(DateTime));

            for (var i=0; i < result.Length; i++)
            {
                table.Rows.Add(usersId[i], result[i],"Hangfire", DateTime.Now, null, null, 1, null, null);
            }

            await using (var transaction =  await _context.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted, cancellationToken))
            {
                try
                {
                    _logger.LogInformation("2 step. Time: {elapsedTime}", timer.Elapsed);
                    context.WriteLine($"2 step - get raw data. Time: {timer.Elapsed}");

                    timer.Restart();

                    _context.UserClusters.RemoveRange(_context.UserClusters);

                    _logger.LogInformation("3 step. Time: {elapsedTime}", timer.Elapsed);
                    context.WriteLine($"3 step - remove old rows from db. Time: {timer.Elapsed}");

                    timer.Restart();

                    var bulk = new SqlBulkCopy((SqlConnection)_context.Database.GetDbConnection(), SqlBulkCopyOptions.Default, (SqlTransaction) transaction.GetDbTransaction());

                    bulk.DestinationTableName = $"dbo.{_context.UserClusters.EntityType.GetDefaultTableName()}s";
                    bulk.BulkCopyTimeout = _context.Database.GetCommandTimeout() ?? bulk.BulkCopyTimeout;

                    foreach (var column in table.Columns.Cast<DataColumn>().Select((value, i) => (value, i)))
                    {
                        bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping()
                        {
                            SourceOrdinal = column.i,
                            DestinationColumn = column.value.ColumnName
                        });
                    }

                    await bulk.WriteToServerAsync(table, cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            timer.Stop();

            _logger.LogInformation("Finished clustering. Time: {elapsedTime}", timer.Elapsed);
            context.WriteLine($"Finished clustering. Time: {timer.Elapsed}");

            return true;
        }
        #endregion

        #region GetPredictions()
        public async Task<List<MovieResultAssign>> GetPredictions(string currentUserId, CancellationToken cancellationToken)
        {
            var movies = await _context.Movies.OrderBy(x => x.Id).Select(x => x.Id).ToListAsync(cancellationToken);

            var currentUserMovieVote = await GetRawDataFromDatabase(movies, new List<string>()
            {
                currentUserId
            }, cancellationToken);

            var currentUserCluster =
                await _context.UserClusters.FirstOrDefaultAsync(x => x.UserId == currentUserId, cancellationToken);

            var usersFromCluster =
                await _context.UserClusters
                    .Where(x => x.ClusterNumber == currentUserCluster.ClusterNumber &&
                                x.UserId != currentUserId &&
                                x.StatusId == 1)
                    .Select(x => new string(x.UserId))
                    .ToListAsync(cancellationToken);

            var moviesVotesFromCluster = await GetRawDataFromDatabase(movies, usersFromCluster, cancellationToken);

            var mostPopularMoviesTable = GetMostPopularMovie(moviesVotesFromCluster, currentUserMovieVote);

            List<MovieResultAssign> movieResults = new List<MovieResultAssign>();

            for (var i = 0; i < mostPopularMoviesTable.Length; i++)
            {
                movieResults.Add(new MovieResultAssign()
                {
                    MovieId = movies[i],
                    Result = mostPopularMoviesTable[i]
                });
            }

            return movieResults;
        }
        #endregion

        #region GetMostPopularMovie()

        private double[] GetMostPopularMovie(double[][] moviesVotes, double[][] currentUserMovieVote)
        {
            var moviesCount = new double [currentUserMovieVote[0].Length];

            for(var i = 0; i < currentUserMovieVote[0].Length; i++)
            {
                for (var j = 0; j < moviesVotes.Length; j++)
                {
                    var userDistance = UserDistance(currentUserMovieVote[0], moviesVotes[j]);
                    var fiFunction = 1 / Math.Pow(userDistance, 2);

                    if (moviesVotes[j][i] > 0)
                    {
                        moviesCount[i] += 1 * fiFunction;
                    }
                }
            }
            return moviesCount;
        }

        #endregion

        #region GetRawDataFromDatabase()

        private async Task<double[][]> GetRawDataFromDatabase(List<int> moviesId, List<string> usersId, CancellationToken cancellationToken)
        {
            var votes = await _context.UserMovieVotes.Where(x => x.StatusId == 1).ToListAsync(cancellationToken);

            var voteUserList = new double[usersId.Count][];
            var i = 0;

            foreach (var userId in usersId)
            {
                var voteMovies = new double[moviesId.Count];
                var y = 0;
                foreach (var movieId in moviesId)
                {
                    var vote = votes.FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId && x.StatusId == 1);

                    if (vote != null)
                    {
                        voteMovies[y] = vote.Vote;
                    }
                    else
                    {
                        voteMovies[y] = 0.0;
                    }
                    y++;
                }

                voteUserList[i] = voteMovies;
                i++;
            }

            return voteUserList;
        }
        #endregion

        #region GetClusters()
        public int[] GetClusters(double[][] rawData, int numClusters)
        {
            var data = rawData;

            var changed = true;
            var success = true; 

            var clusters = InitClustering(data.Length, numClusters);
            var means = InitMeans(numClusters, data[0].Length);

            var maxIteration = data.Length * 10;
            var iteration = 0;
            while (changed 
                   && success 
                   && iteration < maxIteration)
            {
                iteration++;
                success = UpdateMeans(data, clusters, means); 
                changed = UpdateClustering(data, clusters, means); 
            }

            return clusters;
        }
        private int[] InitClustering(int numRecords, int numClusters)
        {
            var random = new Random(numRecords);
            var clustering = new int[numRecords];
            for (var i = 0; i < numClusters; ++i)
                clustering[i] = i;
            for (var i = numClusters; i < clustering.Length; ++i)
                clustering[i] = random.Next(0, numClusters);
            return clustering;
        }

        private double[][] InitMeans(int numClusters, int numColumns)
        {
            double[][] result = new double[numClusters][];
            for (int k = 0; k < numClusters; ++k)
                result[k] = new double[numColumns];
            return result;
        }

        private bool UpdateMeans(double[][] data, int[] clustering, double[][] means)
        {
            var clusterCounts = new int[NUM_CLASTERS];
            //Zliczamy liczebnosc klastrow
            for (var i = 0; i < data.Length; i++)
            {
                var cluster = clustering[i];
                clusterCounts[cluster]++;
            }
            //Jezeli klaster jest pusty to zwracamy false
            for (var k = 0; k < NUM_CLASTERS; k++)
            {
                if (clusterCounts[k] == 0)
                    return false; 
            }
            //Zerjumy srodki
            for (var k = 0; k < means.Length; k++)
            {
                for (var j = 0; j < means[k].Length; j++)
                    means[k][j] = 0.0;
            }
                
            //Dodajemy pola w klastrach
            for (var i = 0; i < data.Length; i++)
            {
                var cluster = clustering[i];
                for (var j = 0; j < data[i].Length; j++)
                    means[cluster][j] += data[i][j];
            }
            //obliczamy nowe srodki dla kazdego atrybutu
            for (var k = 0; k < means.Length; k++)
            {
                for (var j = 0; j < means[k].Length; j++)
                {
                    means[k][j] /= clusterCounts[k];
                }
                    
            }
                
            return true;
        }

        private bool UpdateClustering(double[][] data, int[] clustering, double[][] means)
        {
            var changed = false;

            var newClustering = clustering;

            var distances = new double[NUM_CLASTERS];

            for (var i = 0; i < data.Length; i++)
            {
                for (var k = 0; k < NUM_CLASTERS; k++)
                    distances[k] = Distance(data[i], means[k]);

                var newClusterId = MinIndex(distances); //znajduje najblizszy srodek
                if (newClusterId != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterId;
                }
            }

            if (changed == false)
                return false;

            //zliczamy ilosc wystapien w klastrze
            var clusterCounts = new int[NUM_CLASTERS];
            for (var i = 0; i < data.Length; i++)
            {
                var cluster = newClustering[i];
                clusterCounts[cluster]++;
            }

            for (var k = 0; k < NUM_CLASTERS; k++)
            {
                if (clusterCounts[k] == 0)
                    return false;
            }

            clustering = newClustering;

            return true;
        }

        private double UserDistance(double[] predictionUserVotes, double[] anotherUserFromClasterVotes)
        {
            var sum = 0.0;

            for (var i = 0; i < predictionUserVotes.Length; i++)
            {
                sum += Math.Pow((predictionUserVotes[i] - anotherUserFromClasterVotes[i]), 2);
            }

            return Math.Sqrt(sum);
        }

        private double Distance(double[] record, double[] mean)
        {
            var sum = 0.0;

            for (var i = 0; i < record.Length; ++i)
            {
                sum += Math.Pow((record[i] - mean[i]), 2);
            }

            return Math.Sqrt(sum);
        }

        private int MinIndex(double[] distances)
        {
            var indexOfMinDistance = 0;
            var minDist = distances[0];
            for (var i = 0; i < distances.Length; ++i)
            {
                if (distances[i] < minDist)
                {
                    minDist = distances[i];
                    indexOfMinDistance = i;
                }
            }
            return indexOfMinDistance;
        }
        #endregion

        #region CreateRandomVotes()

        public async Task<bool> CreateRandomVotes(PerformContext context, CancellationToken cancellationToken)
        {
            #region UserGuids
            var userGuids = new List<string>()
            {
                "042f9c20-f903-4c32-adc3-a1b62faa98fb",
                "048f6066-cf29-41dd-bfc3-842bafc23d32",
                "04c6084c-2cc9-41d0-a1b7-bd765c2f1315",
                "051cbbd4-f190-4c94-a9e2-afab99b25553",
                "05ec3e7b-3af4-4d2b-845f-ea9a9b3989e7",
                "138b96a3-3d2d-41f7-87d9-b4c9100fbad2",
                "1425c963-28e1-4258-88fa-3ac672b2bf9b",
                "181e2b19-b88c-4691-800b-92b008f8f54f",
                "1c762625-3ee3-492c-ad4d-169162a1fab3",
                "1e6ad2c4-b081-4422-8afb-73260483d7ff",
                "1e6f4ea6-2d6a-4542-9fe4-e901f924d015",
                "1f436756-0be8-4f5b-a77a-ae82969762c2",
                "21126d13-3e71-48af-9a8e-f725c1ba9570",
                "2938b8dd-f026-46c3-8968-f49f1aaf8abf",
                "2c082982-10c9-446d-9fdb-d5c8a204aeb7",
                "2db3f24e-7e84-49ab-b6e1-5367d51aeccd",
                "2e6139b1-c6c8-4929-81a1-fd23f4465c95",
                "31653ed6-dfa5-4243-8103-e306b375ed53",
                "341010f3-7677-4472-99a9-3cf5478fd4e5",
                "379a33a1-4298-456a-b879-143ee61cdc1e",
                "38713913-850b-4707-b3d9-c27b03f30068",
                "3a31bc6a-295a-4721-ad5d-2d2d7c8bd7a7",
                "3ba26fb6-4b53-495c-bac9-7089c6b473b0",
                "41e2db25-9d66-4f84-9ed2-141f020036e0",
                "444069d1-9de7-4574-9304-9eb691cc58e4",
                "45ef19c2-ab9d-4eb5-aa4c-760a1574e805",
                "47cdff31-6337-4a8d-b2b7-41bdac2d6b56",
                "48bb288c-e3b2-45e2-96a4-44c27591795b",
                "4ab5f17c-8765-4940-b2ef-0377f55b5e5a",
                "4afdafc7-e7d9-48b9-a1b2-23e74414d0da",
                "4dd2b303-b171-4fdc-a35f-906031fdcc78",
                "512331d9-be7b-431d-bf22-6ca495ddc0c6",
                "57e6b64e-adcc-4849-968a-960d39cf5376",
                "5975b45e-3678-4cca-9a79-61b3461e1d06",
                "5a858c4f-dcc1-4ad0-9820-69fd03c6a748",
                "5aedab75-3853-4702-8e13-7f6c7a06a89d",
                "6139956d-0f18-4da8-9f04-8e9b239c9a1d",
                "615cf922-f1a2-4fbc-9476-c301ee273bfe",
                "62d4d2a6-db88-48b9-893c-6ca2301939d3",
                "684df4fc-e8db-4fde-b824-11af474bda72",
                "69161002-781f-472d-a3c7-e1c60e5534ba",
                "6a0c0a19-82f5-4f91-831e-b39f6ea3eb5d",
                "6f09ec7c-3a21-4528-9645-2a8b4c804d89",
                "6fe5d80f-bfd9-4bb8-95dc-ad679b144e4a",
                "70f8887f-f5a9-4b38-936e-4c2bd53b7dd2",
                "7134373d-fe17-4afd-865d-ee3ae005dc84",
                "71c65b02-4a66-435d-9a26-a20fa94c9f50",
                "75a00ecd-2109-4d60-a3e6-daffd42f4e94",
                "7b112e14-067e-4844-906e-4e6cad752938",
                "7bd5230f-6209-4c09-bae7-8956044db44e",
                "7dee0bb3-5940-41f8-82c9-54fde8633ca1",
                "821c5a6c-21bc-4f98-8920-090c599a7ff9",
                "84ee3c36-9675-4880-8549-56abae9417a1",
                "857d350a-810a-418b-bbee-e6d6387c508e",
                "85bbeb24-040e-4736-9ec7-2ad3f907661d",
                "85c83a8f-b2c5-4ff2-9c0a-3afd7b6ec6b3",
                "872a265c-8d94-4ccf-bad3-d5f0ad902d61",
                "89218b46-b7e4-4926-a958-61a7d414a97f",
                "899d902d-9198-4d9a-abb0-156d9a70ef47",
                "8a124230-3bb6-4cff-9e28-ff65943caa87",
                "94399e41-7ef3-4c59-af22-3080b1555cf7",
                "94bd840d-985b-4a00-93df-3f677e672690",
                "98122158-ce9b-4995-ac15-1efceb117658",
                "996f41da-cde4-4dbb-9bf6-0a5ab483b7ac",
                "a1f827c6-7075-4e6a-85a4-51673286c758",
                "a3e0191d-8c62-4723-a843-48c1ed73282f",
                "a61e3e32-dab3-4701-9acd-a4cb6852e539",
                "a69136b4-b78d-429b-97a1-5dd05c5a838e",
                "aae2b836-3c22-4fde-82cb-bea37ba9ea77",
                "adcefbfd-80c2-4025-8748-c1eedd681d69",
                "b4d2cc7b-d434-44b1-af78-d33aa0cbbf0c",
                "b94e9a76-b0c9-4f44-b3e4-3047c4570425",
                "bc9227a1-3479-43a6-86f8-154e87a0a7c5",
                "caea8bec-9cc9-4baf-a218-56389a0f4ad3",
                "d016565d-7b4c-42db-9571-41605d28524a",
                "d08c04b2-cda2-48dd-8fab-6725b1646628",
                "d23d55e3-e24a-4b26-8dc3-fd72f9060e64",
                "d3e3f3e2-4884-40ce-9e1b-2f34fddecfae",
                "d9be0b5c-fc14-4a80-bda0-b639fe033c14",
                "dbfc7fb8-c264-41d2-8d8f-60d41b2c85f5",
                "dc39c79c-e761-4d63-9a81-6ae4c2cdd942",
                "dd618167-04dd-43aa-86a5-0291f8ec9884",
                "df080825-5521-40d1-b932-fffd9e3d7665",
                "e31277ef-83e0-4c8d-aed6-3cc11a03ec9a",
                "e3bcafdc-20a8-4ea1-b9e8-ab4e105677b1",
                "e7d954ef-72ce-4c9c-bc38-7b7706179915",
                "e80d70eb-82da-4396-954f-6f4e1d900912",
                "e90f182f-7b78-4d12-91de-5c581f74626e",
                "e98b6631-c6e2-40bc-9aa5-f1b9e1ddced1",
                "ea5dce0b-4c1a-43d6-8d8d-1336ce88cc26",
                "eb2540b8-2eeb-4ea5-b164-cc3064299a92",
                "ede6fc56-3a61-418c-b247-ed272a0719b2",
                "ee2ab858-13e8-4cab-8cc5-6a1ab3f6bb42",
                "ef8f9a67-7867-44f1-98e3-5a77910f59a5",
                "efb79507-c704-425e-8c52-34d1432d93f6",
                "f0b67d1a-f1a4-47e3-9e34-a8401f30990f",
                "f3f2c4b9-c537-4428-a7d0-89cf067b97f9",
                "f4e8c17b-16e0-4408-9244-d2cb05a023c0",
                "fabed379-fb94-4590-896d-18437a9e705b",
                "fc38ead9-a8ff-4f47-a150-d19a9fba2624",
                "fd1c7300-af17-4fc9-9a17-5286522f5b3e",
                "ffa1cc87-d0bd-433f-93a2-e8fb0ba8cb45"
            };

            #endregion

            context.WriteLine("Rozpoczynam dodawanie głosów");

            var userVotesList = await _context.UserMovieVotes.Where(x=>x.StatusId == 1).ToListAsync(cancellationToken);

            _context.UserMovieVotes.RemoveRange(userVotesList);

            //Pobieramy wszystkie filmy
            var movies = await _context.Movies.Select(x=>x.Id).ToListAsync(cancellationToken);
            Random random = new Random();

            foreach (var userId in userGuids)
            {
                //Pobieramy liczbę losowa z przedziału 55% do 80% liczby wszystkich filmów 
                var low = Convert.ToInt32(0.55 * movies.Count);
                var high = Convert.ToInt32(0.8 * movies.Count);
                var randomCount = random.Next(low, high);
                context.WriteLine($"Losujemy filmy dla usera:{userId}\n" +
                                  $"Dodamy {randomCount} ocen filmów");
                //Sortujemy losowo filmy i pobieramy z góry ilość wylosowaną
                var userMovies = movies.OrderBy(x => Guid.NewGuid()).Take(randomCount).ToList();

                foreach (var movieId in userMovies)
                {
                    var vote = random.Next(1, 5);
                    _context.UserMovieVotes.Add(new UserMovieVote()
                    {
                        MovieId = movieId,
                        Vote = vote,
                        UserId = userId
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            context.WriteLine("Zakończenie dodawania głosów");

            return true;
        }

        #endregion
    }
}
