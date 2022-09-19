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
                "ad19c8c8-225a-4e6d-8f34-83364d662c3c",
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
                "b12a004d-6b43-4217-88e3-f2c127ff7e24",
                "9d54226e-406b-4ccb-9536-abe582ae5c02",
                "22a4a50c-107c-4774-a0f4-11c69570e717",
                "8dcb8178-a80a-41aa-9197-a614ba0a6960",
                "d5280fd9-a745-4107-a8ff-3bc3102b3b95",
                "9e75a6d6-a439-4c43-8cab-cbb2ca2af724",
                "5a32e14a-b427-448b-b0de-1290d6a37bc9",
                "4f7a63df-cb11-4bff-bdf0-30b4e17dfa2a",
                "4d2cff63-74a5-4f8f-ab27-c68abc319689",
                "5f1bb29f-7230-4204-8035-12584a0dc639",
                "f49e693e-1ccf-40e0-9b5e-eebac4597c2d",
                "a9337dd1-e076-4b29-95ed-b8b1dadb7699",
                "306d51cd-607a-4a5f-b5f7-e4f3e51b2e0e",
                "35ff54ec-2ef0-4c11-8e36-50154e0b73f9",
                "0458723b-a715-48e1-8d2e-1b34652560e4",
                "bd1f62da-3121-4b8a-a8e2-3f32dfbb5840",
                "bb210e49-6492-482f-ac1b-b30974afe5f0",
                "563a9c03-9135-4635-8c20-93ec3a27c949",
                "1285a24d-d15e-49c1-905c-2ac50675133d",
                "f3ac8560-2e4a-470d-abec-4fcb1b70383a",
                "7354cb08-35c8-44a2-9ce1-c0571c8ae9c7",
                "a721f481-4e01-4442-97bb-11932409ccef",
                "22a07130-35d0-4f17-baf3-54f635d40a48",
                "0e9933d9-033f-4d0f-8a3d-6d208a75033f",
                "4ad8762a-966e-4048-a8cd-cc6165b94817",
                "986a6182-86a6-4546-ad70-4012dbedc7c9",
                "e938d37c-856c-47f6-8430-77e973c8c781",
                "e9ec4fc4-5883-4c00-b4f9-07d95b5c959f",
                "6e7ad46e-ed37-4d6b-9bde-111f92a8e885",
                "1dbd8073-5d90-409d-a4c0-794fe725172b",
                "a612a3f4-e59a-4d61-8823-9e67e787b141",
                "3d1cf4a0-7d2d-454f-81bc-59fbd67f1a89",
                "24cfc63f-a8f5-41fe-bce0-1d8052d8b5f8",
                "6a90d921-dba9-40bf-9971-e91408db1078",
                "f88b1092-be77-4da3-a08a-09b4be9a6e46",
                "0b66ab2f-9bc4-47c8-bf8b-041253a48aa0",
                "8a1e0292-d1ce-44cd-b8f4-3cabc6a6c937",
                "f18c56bf-f124-4974-bce6-e806ca912da0",
                "b319dde8-8bd5-4bed-bdfe-a9391de45bb4",
                "71a467ff-3100-446a-807d-c645d8a6b928",
                "a76494e3-124f-4abe-a158-7386f5ddf693",
                "e0c1ab19-1375-411d-9f88-1de64e2f9457",
                "c898de18-3301-41a4-8226-77ead1fe39eb",
                "b6cf57b1-dff7-4dfe-b30a-7f6535f8fdbd",
                "25d97baf-4cfc-4779-a7ce-b17c5d669cf1",
                "dbe02889-b249-4c62-802a-2227b8e5bc53",
                "475fce9f-0a59-449e-b79c-6c8eb4902985",
                "de424716-b2fc-4977-ac9a-dc96a737307c",
                "89b95988-b18c-4bdd-9b7f-24bfe6a1be64",
                "afea632e-3066-4b20-9cd9-091b4e0b9c64",
                "36c31474-063d-4467-8ac5-055fe7c5d9f7",
                "a84ca1a9-8fa2-4c3d-83e7-b0b914c0998a",
                "3c73e512-0c5b-4771-b0c3-e85acd523fe0",
                "7dbc0b75-5984-4a86-8c90-40f73f142405",
                "31ccfe4a-ecc4-4f5d-b413-0cdfd9bba898",
                "b40e947c-9fdb-4708-acc0-9b13d1251993",
                "fdee6fc3-8b11-4fce-b79e-5e8a76e7950c",
                "f0889482-4b46-4ebf-92d9-e9e51f3fa9af",
                "d8397611-60b4-4386-8236-b7d6e3b72543",
                "ca43c6b1-64b2-4d45-bada-c6d72f334e34",
                "4091fe7b-c1ad-43cc-811e-cad6a8f0f4a2",
                "9dfc7fca-308d-4679-b2f4-8834b02ec143",
                "ce43c46f-63fb-439a-834d-814eb93e1547",
                "3bb80daf-7894-4b6a-9d70-9399779acc43"
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
