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

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.DeleteSeanceSeat
{
    public class DeleteSeanceSeatCommandHandler : IRequestHandler<DeleteSeanceSeatCommand, Unit>
    {
        private readonly ICinemaDbContext _context;

        public DeleteSeanceSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seanceSeatToDelete = await _context.SeanceSeats.Where(x => x.Id == request.SeanceSeatId && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (seanceSeatToDelete == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            _context.SeanceSeats.Remove(seanceSeatToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
