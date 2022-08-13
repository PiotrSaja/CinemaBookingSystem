using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using CinemaBookingSystem.Domain.Entities;
using Hangfire.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CinemaBookingSystem.Api.Services
{
    public class UserVoteService : IUserVoteService
    {
        private readonly int NUM_CLASTERS = 3;
        private readonly ICinemaDbContext _context;
        private readonly ILogger<UserVoteService> _logger;

        #region UserVoteService()

        public UserVoteService(ICinemaDbContext context, ILogger<UserVoteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #endregion

        #region Clustering()
        public async Task<bool> Clustering(CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            _logger.LogInformation("Start clustering");
            _logger.LogInformation("Job running at: {time}", DateTimeOffset.UtcNow);

            timer.Start();

            var movies = await _context.Movies.OrderBy(x => x.Id).Select(x=>x.Id).ToListAsync(cancellationToken);

            var usersId = await _context.UserMovieVotes.Select(x => x.UserId).Distinct().ToListAsync(cancellationToken);

            _logger.LogInformation("1 step. Time: {elapsedTime}", timer.Elapsed);

            timer.Restart();

            var rawDataFromDb = await GetRawDataFromDatabase(movies, usersId, cancellationToken);

            int[] result = GetClusters(rawDataFromDb, NUM_CLASTERS);

            _context.UserClusters.RemoveRange(_context.UserClusters);

            _logger.LogInformation("2 step. Time: {elapsedTime}", timer.Elapsed);

            timer.Restart();

            var userClusters =
                new Dictionary<string, int>();

            //Przerobić na bulk copy
            for (var i=0; i < result.Length; i++)
            {
                userClusters.Add(usersId[i], result[i]);

                _context.UserClusters.Add(new UserCluster()
                {
                    UserId = usersId[i],
                    ClusterNumber = result[i]
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
            timer.Stop();

            _logger.LogInformation("Finished clustering. Time: {elapsedTime}", timer.Elapsed);

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
                await _context.UserClusters.Where(x => x.ClusterNumber == currentUserCluster.ClusterNumber && x.UserId != currentUserId && x.StatusId == 1).Select(x => new string(x.UserId)).ToListAsync(cancellationToken);


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
        //Bardzo wolna metoda - pomyslec nad zmniejszeniem ilosci zapytań
        #region GetRawDataFromDatabase()
        private async Task<double[][]> GetRawDataFromDatabase(List<int> moviesId, List<string> usersId, CancellationToken cancellationToken)
        {
            var voteUserList = new double[usersId.Count][];
            var i = 0;

            foreach (var userId in usersId)
            {
                var voteMovies = new double[moviesId.Count];
                var y = 0;
                foreach (var movieId in moviesId)
                {
                    var vote = await _context.UserMovieVotes.FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId, cancellationToken);

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

        private double[][] NormalizedData(double[][] rawData)
        {
            // normalize raw data by computing (x - mean) / stddev
            // primary alternative is min-max:
            // v' = (v - min) / (max - min)

            // make a copy of input data
            double[][] result = new double[rawData.Length][];
            for (int i = 0; i < rawData.Length; ++i)
            {
                result[i] = new double[rawData[i].Length];
                Array.Copy(rawData[i], result[i], rawData[i].Length);
            }

            for (int j = 0; j < result[0].Length; ++j) // each col
            {
                double colSum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    colSum += result[i][j];
                double mean = colSum / result.Length;
                double sum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    sum += (result[i][j] - mean) * (result[i][j] - mean);
                double sd = sum / result.Length;
                for (int i = 0; i < result.Length; ++i)
                    result[i][j] = (result[i][j] - mean) / sd;
            }
            return result;
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
