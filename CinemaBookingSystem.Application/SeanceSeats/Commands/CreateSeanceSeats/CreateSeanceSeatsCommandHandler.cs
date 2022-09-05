using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeats
{
    public class CreateSeanceSeatsCommandHandler : IRequestHandler<CreateSeanceSeatsCommand, bool>
    {
        private readonly ICinemaDbContext _context;

        #region CreateSeanceSeatsCommandHandler()
        public CreateSeanceSeatsCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<bool> Handle(CreateSeanceSeatsCommand request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances
                .FirstOrDefaultAsync(x => x.Id == request.SeanceId, cancellationToken);

            if (seance == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists seance in database, check your SeanceId");

            foreach (var seanceSeat in request.SeanceSeats)
            {
                var newSeanceSeat = new SeanceSeat()
                {
                    CinemaSeatId = seanceSeat.CinemaSeatId,
                    Price = seanceSeat.Price,
                    SeatStatus = false,
                    SeanceId = seance.Id
                };
                _context.SeanceSeats.Add(newSeanceSeat);
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        #endregion
    }
}
