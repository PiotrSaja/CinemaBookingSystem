using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Extensions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesContentBasedPrediction
{
    public class GetMoviesContentBasedPredictionQueryHandler : IRequestHandler<GetMoviesContentBasedPredictionQuery, MoviesDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        #region GetMoviesContentBasedPredictionQueryHandler()
        public GetMoviesContentBasedPredictionQueryHandler(ICinemaDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }
        #endregion

        #region Handle()

        public async Task<MoviesDetailVm> Handle(GetMoviesContentBasedPredictionQuery request, CancellationToken cancellationToken)
        {
            if (request.PageSize < 1 && request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size and Page index can't be null or less than 1"); }
            if (request.PageSize < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page size can't be null or less than 1"); }
            if (request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Page index can't be null or less than 1"); }

            var moviesPref = await _context.UserPreferencesMovies
                .Include(x=>x.Movie)
                .ThenInclude(x=>x.Actors)
                .Include(x=>x.Movie)
                .ThenInclude(x=>x.Director)
                .Include(x=>x.Movie)
                .ThenInclude(x=>x.Genres)
                .Where(x => x.UserId == _userService.Id)
                .ToListAsync(cancellationToken);

            //Prepare user profile for content base filtering
            var userProfile = new UserProfile();
            var counter = 0;

            foreach (var moviePref in moviesPref)
            {
                double imdbRate;
                if (Double.TryParse(moviePref.Movie.ImdbRating, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"),
                        out imdbRate))
                {
                    userProfile.ImdbVoteAvg += imdbRate;
                    counter++;
                }
                
                if (!userProfile.DirectorsString.Contains(moviePref.Movie.Director.DirectorName.ToString()))
                    userProfile.DirectorsString += moviePref.Movie.Director.DirectorName.ToString();

                foreach (var genre in moviePref.Movie.Genres)
                {
                    if (!userProfile.GenresString.Contains(genre.Name))
                        userProfile.GenresString += genre.Name;
                }

                foreach (var actor in moviePref.Movie.Actors)
                {
                    if (!userProfile.ActorsString.Contains(actor.ActorName.ToString()))
                        userProfile.ActorsString += actor.ActorName.ToString();
                }
            }

            userProfile.ImdbVoteAvg /= counter;
            userProfile.DirectorsString = RemoveWhitespace(userProfile.DirectorsString);
            userProfile.GenresString = RemoveWhitespace(userProfile.GenresString);
            userProfile.ActorsString = RemoveWhitespace(userProfile.ActorsString);

            var predicate = PredicateBuilder.New<Movie>(true);

            predicate.And(x => !moviesPref.Select(x => x.MovieId).Contains(x.Id));

            var movies = await _context.Movies.Where(predicate)
                .Include(x=>x.Director)
                .Include(x=>x.Actors)
                .Include(x=>x.Genres)
                .ToListAsync(cancellationToken);

            var result = new List<ResultModel>();

            foreach (var movie in movies)
            {
                var imdbRate = 0.0d;
                //If movie imdb rate < userProfile.ImdbVoteAvg skip or don't have valid value
                if (Double.TryParse(movie.ImdbRating, NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"),
                        out imdbRate))
                {
                    if (imdbRate < userProfile.ImdbVoteAvg)
                        continue;
                }
                else
                    continue;

                var directorDist = CalculateSimilarity(RemoveWhitespace(movie.Director.DirectorName.ToString()),
                    userProfile.DirectorsString);

                var genres = "";
                var actors = "";

                foreach (var genre in movie.Genres)
                    genres += genre.Name;
                foreach (var actor in movie.Actors)
                    actors += actor.ActorName.ToString();

                var actorsDist = CalculateSimilarity(RemoveWhitespace(actors), userProfile.ActorsString);
                var genreDist = CalculateSimilarity(RemoveWhitespace(genres), userProfile.GenresString);

                result.Add(new ResultModel()
                {
                    ActorDistance = actorsDist,
                    DirectorDistance = directorDist,
                    GenreDistance = genreDist,
                    Movie = movie
                });
            }

            var orderedMovie = result.OrderByDescending(x => x.GenreDistance)
                .ThenByDescending(x => x.ActorDistance)
                .ThenByDescending(x => x.DirectorDistance)
                .Select(x=> x.Movie)
                .Paginate(request.PageIndex, request.PageSize, cancellationToken);

            var moviesDto = _mapper.Map<List<Movie>, List<MovieDetailDto>>(orderedMovie.Items.ToList());

            var moviesDetailVm = new MoviesDetailVm()
            {
                CurrentPage = orderedMovie.CurrentPage,
                TotalItems = orderedMovie.TotalItems,
                TotalPages = orderedMovie.TotalPages,
                Items = moviesDto
            };

            return moviesDetailVm;
        }
        #endregion

        #region LevenshteinDistance()
        private int LevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
                return 0;
            if (String.IsNullOrEmpty(a))
                return b.Length;
            if (String.IsNullOrEmpty(b))
                return a.Length;

            var lengthA = a.Length;
            var lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (var i = 0; i <= lengthA; distances[i, 0] = i++);
            for (var j = 0; j <= lengthB; distances[0, j] = j++);

            for (var i = 1; i <= lengthA; i++)
            for (var j = 1; j <= lengthB; j++)
            {
                var cost = b[j - 1] == a[i - 1] ? 0 : 1;
                distances[i, j] = Math.Min
                (
                    Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                    distances[i - 1, j - 1] + cost
                );
            }
            return distances[lengthA, lengthB];
        }
        #endregion

        #region CalculateSimilarity()
        private double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = LevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
        #endregion 

        #region RemoveWhitespace()
        private string RemoveWhitespace(string text) => String.Concat(text.Where(x => !Char.IsWhiteSpace(x)));

        #endregion
    }
    #region UserProfile()
    public class UserProfile
    {
        public double ImdbVoteAvg { get; set; }
        public string ActorsString { get; set; }
        public string DirectorsString { get; set; }
        public string GenresString { get; set; }
        public string PlotKeywords { get; set; }

        public UserProfile()
        {
            ImdbVoteAvg = 0;
            ActorsString = "";
            DirectorsString = "";
            GenresString = "";
            PlotKeywords = "";
        }
    }
    #endregion

    #region ResultModel
    public class ResultModel
    {
        public double DirectorDistance { get; set; }
        public double ActorDistance { get; set; }
        public double GenreDistance { get; set; }
        public Movie Movie { get; set; }

    }
    #endregion
}
