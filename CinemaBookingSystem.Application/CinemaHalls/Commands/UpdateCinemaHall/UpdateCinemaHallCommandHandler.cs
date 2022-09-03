using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.UpdateCinemaHall
{
    public class UpdateCinemaHallCommandHandler : IRequestHandler<UpdateCinemaHallCommand, int>
    {
        private readonly ICinemaDbContext _context;

        #region UpdateCinemaHallCommandHandler()
        public UpdateCinemaHallCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(UpdateCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var cinemaHallToUpdate = await _context.CinemaHalls
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (cinemaHallToUpdate == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            var cinema = await _context.Cinemas
                .FirstOrDefaultAsync(x => x.Id == request.CinemaId, cancellationToken);

            if (cinema == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema in database, check your CinemaId");

            cinemaHallToUpdate.Name = request.Name;
            cinemaHallToUpdate.TotalSeats = request.TotalSeats;
            cinemaHallToUpdate.CinemaId = request.CinemaId;
            cinemaHallToUpdate.NumberOfColumns = request.NumberOfColumns;
            cinemaHallToUpdate.NumberOfRows = request.NumberOfRows;

            _context.CinemaHalls.Update(cinemaHallToUpdate);
            await _context.SaveChangesAsync(cancellationToken);

            return cinemaHallToUpdate.Id;
        }
        #endregion
    }
}
