﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Application.Common.Models;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.ValueObjects;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.CreateMovieFromExternalApi
{
    public class CreateMovieFromExternalApiCommandHandler : IRequestHandler<CreateMovieFromExternalApiCommand, int>
    {
        private readonly ICinemaDbContext _context;
        private readonly IOmdbClient _omdbClient;
        private readonly ITmdbClient _tmdbClient;

        #region CreateMovieFromExternalApiCommandHandler()
        public CreateMovieFromExternalApiCommandHandler(ICinemaDbContext context, IOmdbClient omdbClient, ITmdbClient tmdbClient)
        {
            _context = context;
            _omdbClient = omdbClient;
            _tmdbClient = tmdbClient;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(CreateMovieFromExternalApiCommand request, CancellationToken cancellationToken)
        {
            var importedMovieJson = await _omdbClient.GetMovieById(request.ImdbId, cancellationToken);

            var movieFromApi = JsonSerializer.Deserialize<MovieModel>(importedMovieJson);

            if (movieFromApi.Title == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in omdb database.");

            var movieFromTmdb = await _tmdbClient.GetMovieByImdbId(request.ImdbId, cancellationToken);

            MovieTmdbJson movieTmdbJson =
                JsonSerializer.Deserialize<MovieTmdbJson>(movieFromTmdb);

            string[] releasedDate = movieFromApi.Released.Split(" ");
            var releasedDay = Int32.Parse(releasedDate[0]);
            var releasedMonth = DateTime.ParseExact(releasedDate[1], "MMM", CultureInfo.InvariantCulture).Month;
            var releasedYear = Int32.Parse(releasedDate[2]);
            
            var genres = movieFromApi.Genre.Split(", ").ToList();
            var genresList = new List<Genre>();
            foreach (var item in genres)
            {
                var genre = _context.Genres.FirstOrDefault(x => x.Name.Equals(item));

                if (genre != null)
                {
                    genresList.Add(genre);
                }
                else
                {
                    var newGenre = new Genre()
                    {
                        Name = item,
                        Description = ""
                    };
                    genresList.Add(newGenre);
                }
            }
            
            var actors = movieFromApi.Actors.Split(", ").ToList();
            var actorsList = new List<Actor>();
            foreach (var item in actors)
            {
                var splitedItem = item.Split(" ");
                var firstName = "";
                var lastName = "";
                if (splitedItem.Length == 1)
                {
                    firstName = splitedItem[0];
                    lastName = "";
                }
                else if (splitedItem.Length > 2)
                {
                    firstName = splitedItem[0];
                    lastName = splitedItem[1] + " " + splitedItem[2];
                }
                else
                {
                    firstName = splitedItem[0];
                    lastName = splitedItem[1];
                }
                var actor = _context.Actors.FirstOrDefault(x => x.ActorName.FirstName.Equals(firstName) && x.ActorName.LastName.Equals(lastName));

                if (actor == null)
                {
                    actor = new Actor()
                    {
                        ActorName = new PersonalName()
                        {
                            FirstName = firstName,
                            LastName = lastName
                        }
                    };
                }
                actorsList.Add(actor);
            }

            var directorFirstNameFromApi = movieFromApi.Director.Split(" ")[0];
            var directorLastNameFromApi = movieFromApi.Director.Split(" ")[1];

            var director = _context.Directors.FirstOrDefault(x => x.DirectorName.FirstName.Equals(directorFirstNameFromApi) && x.DirectorName.LastName.Equals(directorLastNameFromApi));
            if (director == null)
            {
                director = new Director()
                {
                    DirectorName = new PersonalName()
                    {
                        FirstName = directorFirstNameFromApi,
                        LastName = directorLastNameFromApi
                    }
                };
            }
            
            if (movieFromApi.Runtime.Equals("N/A"))
                movieFromApi.Runtime = "0";

            Movie movie = new()
            {
                Title = movieFromApi.Title,
                Plot = movieFromApi.Plot,
                Language = movieFromApi.Language,
                Duration = Int32.Parse(movieFromApi.Runtime.Split(" ")[0]),
                Released = new DateTime(releasedYear, releasedMonth, releasedDay),
                Country = movieFromApi.Country,
                Genres = genresList,
                Director = director,
                Actors = actorsList,
                PosterPath = movieFromApi.Poster,
                ImdbRating = movieFromApi.imdbRating,
                BackgroundImagePath = "https://image.tmdb.org/t/p/original" + movieTmdbJson.movie_results[0].backdrop_path
            };
            _context.Movies.Add(movie);

            await _context.SaveChangesAsync(cancellationToken);

            return movie.Id;
        }
        #endregion
    }
}