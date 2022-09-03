using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.CreateCinemaHall
{
    public class CreateCinemaHallCommandHandler : IRequestHandler<CreateCinemaHallCommand, int>
    {
        private readonly ICinemaDbContext _context;

        #region CreateCinemaHallCommandHandler()
        public CreateCinemaHallCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()

        public async Task<int> Handle(CreateCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(x => x.Id == request.CinemaId, cancellationToken);

            if (cinema == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema in database, check your CinemaId");

            var cinemaHall = new CinemaHall()
            {
                Name = request.Name,
                TotalSeats = request.TotalSeats,
                CinemaId = request.CinemaId,
                NumberOfColumns = request.NumberOfColumns,
                NumberOfRows = request.NumberOfRows
            };

            _context.CinemaHalls.Add(cinemaHall);

            await _context.SaveChangesAsync(cancellationToken);

            return cinemaHall.Id;
        }

        #endregion
    }
}
