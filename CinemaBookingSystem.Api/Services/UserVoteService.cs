using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using CinemaBookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Api.Services
{
    public class UserVoteService : IUserVoteService
    {
        private readonly int NUM_CLASTERS = 3;
        private readonly ICinemaDbContext _context;

        #region UserVoteService()
        public UserVoteService(ICinemaDbContext context)
        {
            this._context = context;
        }
        #endregion

        #region Clustering()
        public async Task<bool> Clustering(CancellationToken cancellationToken)
        {
            var movies = _context.Movies.OrderBy(x => x.Id).Select(x=>x.Id).ToList();

            var usersId = _context.UserMovieVotes.Select(x => x.UserId).Distinct().ToList();

            var rawDataFromDb = GetRawDataFromDatabase(movies, usersId);
            ShowData(rawDataFromDb, 1, true, true);


            //double[][] rawData = new double[20][];
            //rawData[0] = new double[] { 4.5, 5.0, 3.5, 4.0, 3.0 };
            //rawData[1] = new double[] { 4.0, 4.0, 4.5, 4.0, 3.5 };
            //rawData[2] = new double[] { 2.0, 3.0, 2.5, 5.0, 3.5 };
            //rawData[3] = new double[] { 5.0, 2.0, 3.5, 2.0, 2.5 };
            //rawData[4] = new double[] { 5.0, 4.0, 2.5, 1.0, 3.0 };
            //rawData[5] = new double[] { 5.0, 3.0, 3.5, 3.0, 3.5 };
            //rawData[6] = new double[] { 4.5, 4.0, 3.5, 2.0, 4.5 };
            //rawData[7] = new double[] { 4.0, 4.0, 4.5, 4.0, 3.5 };
            //rawData[8] = new double[] { 3.0, 4.0, 5.0, 4.0, 4.0 };
            //rawData[9] = new double[] { 4.0, 5.0, 4.5, 5.0, 3.5 };
            //rawData[10] = new double[] { 4.0, 2.0, 2.5, 1.0, 2.5 };
            //rawData[11] = new double[] { 2.0, 1.0, 3.5, 1.0, 1.5 };
            //rawData[12] = new double[] { 5.0, 5.0, 5.0, 5.0, 5.0 };
            //rawData[13] = new double[] { 4.0, 4.0, 4.0, 4.0, 4.0 };
            //rawData[14] = new double[] { 3.0, 3.0, 3.0, 3.0, 3.0 };
            //rawData[15] = new double[] { 3.5, 4.0, 3.5, 4.0, 3.5 };
            //rawData[16] = new double[] { 1.0, 2.0, 3.5, 4.0, 5.0 };
            //rawData[17] = new double[] { 3.0, 4.0, 5.0, 1.0, 2.5 };
            //rawData[18] = new double[] { 4.0, 4.5, 3.5, 2.0, 1.5 };
            //rawData[19] = new double[] { 5.0, 3.0, 2.5, 3.0, 3.5 };

            Console.WriteLine("Raw data:\n");
            ShowData(rawDataFromDb, 1, true, true);

            int[] result = GetClusters(rawDataFromDb, NUM_CLASTERS);

            Console.WriteLine("Raw data by cluster:\n");
            ShowClustered(rawDataFromDb, result, NUM_CLASTERS, 1);

            return true;
        }
        #endregion

        #region GetPredictions()
        public Task<List<int>> GetPredictions(CancellationToken cancellationToken)
        {
            /***
             * Z klastrów trzeba wybrać najbardziej liczną wartość za pomocą miary ważonej odległości oraz wyboru użytwkoników w klastrze
             */
            throw new NotImplementedException();
        }
        #endregion

        #region GetRawDataFromDatabase()
        private double[][] GetRawDataFromDatabase(List<int> moviesId, List<string> usersId)
        {
            var voteUserList = new double[usersId.Count][];
            var i = 0;

            foreach (var userId in usersId)
            {
                var voteMovies = new double[moviesId.Count];
                var y = 0;
                foreach (var movieId in moviesId)
                {
                    var vote = _context.UserMovieVotes.FirstOrDefault(x => x.UserId == userId && x.MovieId == movieId);

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
            //double[][] data = NormalizedData(rawData); // normalizacja danych do napisania
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

        #region ShowDate - to delete

        void ShowData(double[][] data, int decimals, bool indices, bool newLine)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                if (indices) Console.Write(i.ToString().PadLeft(3) + " ");
                for (int j = 0; j < data[i].Length; ++j)
                {
                    if (data[i][j] >= 0.0) Console.Write(" ");
                    Console.Write(data[i][j].ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine) Console.WriteLine("");
        }

        void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals)
        {
            for (int k = 0; k < numClusters; ++k)
            {
                Console.WriteLine("------------------------------------");
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    for (int j = 0; j < data[i].Length; ++j)
                    {
                        if (data[i][j] >= 0.0) Console.Write(" ");
                        Console.Write(data[i][j].ToString("F" + decimals) + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("------------------------------------------");
            }
        }

        #endregion

    }
}
