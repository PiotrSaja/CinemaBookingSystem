using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Commands.DeleteMovie
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand>
    {
        private readonly ICinemaDbContext _context;
        public DeleteMovieCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            Movie movie = await _context.Movies.Where(x => x.Id == request.MovieId && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (movie == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            _context.Movies.Remove(movie);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
