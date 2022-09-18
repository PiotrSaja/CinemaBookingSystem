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
                "ed30a7cc-2f62-4570-b3cc-20443f6b7b4e",
                "5e8376d9-798f-4734-8bfb-0366922cd2d2",
                "5dcad384-7f39-49e4-aa73-cc6f732fc974",
                "93a3924a-1aeb-4085-9e30-bf264509b8b3",
                "b2935727-7f1e-4730-a94d-845543675f8e",
                "b2e895d2-f5da-4232-810f-a4ba64466301",
                "6a24743a-a545-4b63-a180-83de36961b93",
                "d1d56a8c-3987-424c-bb43-2edc5ad8f953",
                "a773c633-c911-4af5-b12e-0ecc0b487b3e",
                "579b55f3-7421-4d16-a594-79616f072a73",
                "24debf27-48b1-4a65-ac89-0941c6884375",
                "d67f9ced-229b-4d71-bcc4-cb1c567a0c45",
                "f3287d75-5509-4b1d-bb77-e9b667941f3b",
                "03f63114-8a0b-4134-b070-4dfa4ad669cf",
                "d95d9a34-67c4-454d-aa60-01a4913acf29",
                "51b2630e-7b5d-48cf-981f-2808b24476f7",
                "e1befcdb-1221-4424-9a07-291d1c0c42da",
                "92335f16-9887-4186-8d2f-98b763dd76ea",
                "4585fc6e-5052-4e47-94b8-3447ec7d6a62",
                "bac1532b-e170-4448-8dab-fcc530d82b16",
                "48f315f7-fc57-4554-9172-eae1e3cd8502",
                "a2cf1d46-0d6f-4e48-9e96-552d9a2d989e",
                "87a3e9e5-821a-438d-afa5-d154780f8d1c",
                "e286bcda-d618-4bf8-941c-4eb503558e00",
                "ba243e4f-45c7-4fae-a36e-303e04213d9e",
                "43777305-f92b-4352-b106-53eb8fc02581",
                "aa96ca1d-5125-4f09-a625-35be40e8c3d7",
                "cbe39c4b-392c-4d09-aa0d-d14c8b3ca045",
                "bd6c7c00-bb5d-4f27-97bd-cfe3ef2b3a80",
                "9ce8346b-fb7d-4eed-8e54-0d29133b75f4",
                "891701ba-5442-4444-85ee-ffc27242fbcf",
                "bd7380c8-2199-4df4-b177-af81163acbdc",
                "bfb8f8fb-2e6f-4201-9e28-f7679282c80b",
                "83f85dbc-25c8-4874-9c0a-02fd9f2567f6",
                "b672f10c-466e-4e2f-8c71-5473691e732f",
                "5c7a6b79-1385-4fdc-9330-69cac848e359",
                "b4f0b903-49ec-4f20-bde2-514b1f7ddafd",
                "5f1bb29f-7230-4204-8035-12584a0dc639",
                "7354cb08-35c8-44a2-9ce1-c0571c8ae9c7",
                "3d1cf4a0-7d2d-454f-81bc-59fbd67f1a89",
                "c898de18-3301-41a4-8226-77ead1fe39eb",
                "7dbc0b75-5984-4a86-8c90-40f73f142405",
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
