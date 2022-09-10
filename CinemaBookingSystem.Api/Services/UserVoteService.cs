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

            var usersId = await _context.UserMovieVotes.Select(x => x.UserId).Distinct().ToListAsync(cancellationToken);

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

            for (var i =0; i < mostPopularMoviesTable.Length; i++)
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
            var moviesCount = new double [moviesVotes[0].Length];

            for(var i = 0; i < moviesVotes.Length; i++)
            {
                var userDistance = UserDistance(currentUserMovieVote[0], moviesVotes[i]);
                userDistance = 1 / Math.Pow(userDistance, 2);
                for (var j = 0; j < moviesVotes[i].Length; j++)
                {
                    if (moviesVotes[i][j] > 0)
                    {
                        moviesCount[j] += 1 * userDistance;
                    }
                }
            }
            return moviesCount;
        }

        #endregion

        #region GetRawDataFromDatabase()

        private async Task<double[][]> GetRawDataFromDatabase(List<int> moviesId, List<string> usersId, CancellationToken cancellationToken)
        {
            var votes = await _context.UserMovieVotes.ToListAsync(cancellationToken);

            var voteUserList = new double[usersId.Count][];
            var i = 0;

            foreach (var userId in usersId)
            {
                var voteMovies = new double[moviesId.Count];
                var y = 0;
                foreach (var movieId in moviesId)
                {
                    var vote = votes.FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);

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
    }
}
