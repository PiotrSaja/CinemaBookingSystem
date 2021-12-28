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

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.DeleteCinemaHall
{
    public class DeleteCinemaHallCommandHandler : IRequestHandler<DeleteCinemaHallCommand, Unit>
    {
        private readonly ICinemaDbContext _context;

        public DeleteCinemaHallCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var cinemaHallToDelete = await _context.CinemaHalls.Where(x => x.Id == request.CinemaHallId && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinemaHallToDelete == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }
            _context.CinemaHalls.Remove(cinemaHallToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
