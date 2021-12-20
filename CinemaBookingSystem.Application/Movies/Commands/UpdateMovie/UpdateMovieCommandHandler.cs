using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Commands.UpdateMovie
{
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public UpdateMovieCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies.Where(x => x.Id == request.MovieId)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (movie == null)
            {
                throw new Exception();
            }

            var genresList = await _context.Genres.Where(x => request.Genres.Contains(x.Id)).ToListAsync(cancellationToken);
            var actorsList = await _context.Actors.Where(x => request.Actors.Contains(x.Id)).ToListAsync(cancellationToken);

            movie.Title = request.Title;
            movie.Released = request.Released;
            movie.Duration = request.Duration;
            movie.Genres = genresList;
            movie.Actors = actorsList;
            movie.DirectorId = request.DirectorId;
            movie.Plot = request.Plot;
            movie.Country = request.Country;
            movie.Language = request.Language;
            movie.PosterPath = request.PosterPath;
            movie.ImdbRating = request.ImdbRating;

            _context.Movies.Update(movie);

            await _context.SaveChangesAsync(cancellationToken);

            return movie.Id;
        }
    }
}
