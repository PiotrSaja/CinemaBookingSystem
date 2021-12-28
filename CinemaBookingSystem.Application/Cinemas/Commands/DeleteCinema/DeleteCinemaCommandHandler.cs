using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Cinemas.Commands.DeleteCinema
{
    public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaCommand, Unit>
    {
        private readonly ICinemaDbContext _context;

        public DeleteCinemaCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCinemaCommand request, CancellationToken cancellationToken)
        {
            var cinemaToDelete = await _context.Cinemas.Where(
                    x => x.Id == request.CinemaId
                         && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinemaToDelete == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            _context.Cinemas.Remove(cinemaToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
