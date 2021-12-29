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

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.UpdateSeanceSeat
{
    public class UpdateSeanceSeatCommandHandler : IRequestHandler<UpdateSeanceSeatCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public UpdateSeanceSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seanceSeatToUpdate = await _context.SeanceSeats.Where(x => x.Id == request.SeanceSeatId)
                .FirstOrDefaultAsync(cancellationToken);

            if (seanceSeatToUpdate == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            var seance = await _context.Seances.Where(x => x.Id == request.SeanceId)
                .FirstOrDefaultAsync(CancellationToken.None);
            var cinemaSeat = await _context.CinemaSeats.Where(x => x.Id == request.CinemaSeatId)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (seance == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists seance in database, check your seanceId");
            }
            if (cinemaSeat == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema seat in database, check your CinemaSeatId");
            }

            seanceSeatToUpdate.Price = request.Price;
            seanceSeatToUpdate.CinemaSeatId = request.CinemaSeatId;
            seanceSeatToUpdate.SeanceId = request.SeanceId;
            seanceSeatToUpdate.SeatStatus = request.SeatStatus;

            _context.SeanceSeats.Update(seanceSeatToUpdate);

            await _context.SaveChangesAsync(cancellationToken);

            return seanceSeatToUpdate.Id;
        }
    }
}
