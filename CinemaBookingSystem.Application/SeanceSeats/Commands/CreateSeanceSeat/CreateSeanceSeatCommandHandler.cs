using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeat
{
    public class CreateSeanceSeatCommandHandler : IRequestHandler<CreateSeanceSeatCommand, int>
    {
        private readonly ICinemaDbContext _context;

        #region CreateSeanceSeatCommandHandler()
        public CreateSeanceSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(CreateSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances
                .FirstOrDefaultAsync(x => x.Id == request.SeanceId, cancellationToken);

            var cinemaSeat = await _context.CinemaSeats
                .FirstOrDefaultAsync(x => x.Id == request.CinemaSeatId, cancellationToken);

            if (seance == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists seance in database, check your seanceId");
            if (cinemaSeat == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema seat in database, check your CinemaSeatId");

            var newSeanceSeat = new SeanceSeat()
            {
                CinemaSeatId = request.CinemaSeatId,
                SeanceId = request.SeanceId,
                Price = request.Price,
                SeatStatus = false,
            };

            _context.SeanceSeats.Add(newSeanceSeat);

            await _context.SaveChangesAsync(cancellationToken);

            return newSeanceSeat.Id;
        }
        #endregion
    }
}
