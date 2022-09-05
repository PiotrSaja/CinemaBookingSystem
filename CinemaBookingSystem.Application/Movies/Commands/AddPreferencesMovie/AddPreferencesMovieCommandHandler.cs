using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Commands.AddPreferencesMovie
{
    public class AddPreferencesMovieCommandHandler : IRequestHandler<AddPreferencesMovieCommand, int>
    {
        private readonly ICinemaDbContext _context;
        private readonly IUserService _userService;

        #region AddPreferencesMovieCommandHandler()
        public AddPreferencesMovieCommandHandler(ICinemaDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(AddPreferencesMovieCommand request, CancellationToken cancellationToken)
        {
            UserPreferencesMovie userPreferencesMovie = null;

            foreach (var movieId in request.MoviesIds)
            {
                var movie = await _context.Movies
                    .FirstOrDefaultAsync(x => x.Id == movieId, cancellationToken);

                if (movie == null)
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

                var userMovie =
                    await _context.UserPreferencesMovies.FirstOrDefaultAsync(x =>
                        x.MovieId == movieId && x.UserId == _userService.Id, cancellationToken);

                if (userMovie != null)
                    throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "You cannot add preferences of movies");

                userPreferencesMovie = new UserPreferencesMovie()
                {
                    MovieId = movie.Id,
                    UserId = _userService.Id,
                };

                await _context.UserPreferencesMovies.AddAsync(userPreferencesMovie, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return userPreferencesMovie.Id;
        }
        #endregion
    }
}
