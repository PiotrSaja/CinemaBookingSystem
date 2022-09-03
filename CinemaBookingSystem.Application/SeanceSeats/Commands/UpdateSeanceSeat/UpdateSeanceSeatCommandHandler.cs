using System.Net;
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

        #region UpdateSeanceSeatCommandHandler()
        public UpdateSeanceSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(UpdateSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seanceSeatToUpdate = await _context.SeanceSeats
                .FirstOrDefaultAsync(x => x.Id == request.SeanceSeatId, cancellationToken);

            if (seanceSeatToUpdate == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            var seance = await _context.Seances
                .FirstOrDefaultAsync(x => x.Id == request.SeanceId, cancellationToken);

            var cinemaSeat = await _context.CinemaSeats
                .FirstOrDefaultAsync(x => x.Id == request.CinemaSeatId, cancellationToken);

            if (seance == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists seance in database, check your seanceId");
            if (cinemaSeat == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema seat in database, check your CinemaSeatId");

            seanceSeatToUpdate.Price = request.Price;
            seanceSeatToUpdate.CinemaSeatId = request.CinemaSeatId;
            seanceSeatToUpdate.SeanceId = request.SeanceId;
            seanceSeatToUpdate.SeatStatus = request.SeatStatus;

            _context.SeanceSeats.Update(seanceSeatToUpdate);

            await _context.SaveChangesAsync(cancellationToken);

            return seanceSeatToUpdate.Id;
        }
        #endregion
    }
}
