using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.CreateCinemaSeats
{
    public class CreateCinemaSeatsCommandHandler : IRequestHandler<CreateCinemaSeatsCommand, bool>
    {
        private readonly ICinemaDbContext _context;

        #region CreateCinemaSeatsCommandHandler()
        public CreateCinemaSeatsCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<bool> Handle(CreateCinemaSeatsCommand request, CancellationToken cancellationToken)
        {
            var cinemaHall = await _context.CinemaHalls.FirstOrDefaultAsync(x => x.Id == request.CinemaHallId, cancellationToken);

            if (cinemaHall == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema hall in database, check your CinemaHallId");

            foreach (var cinemaSeat in request.CinemaSeats)
            {
                var newCinemaSeat = new CinemaSeat()
                {
                    SeatNumber = cinemaSeat.SeatNumber,
                    Row = cinemaSeat.Row,
                    SeatType = cinemaSeat.SeatType,
                    CinemaHallId = cinemaSeat.CinemaHallId
                };
                _context.CinemaSeats.Add(newCinemaSeat);
            }
            
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        #endregion
    }
}
