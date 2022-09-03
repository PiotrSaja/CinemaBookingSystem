using System.Net;
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

        #region DeleteCinemaHallCommandHandler()
        public DeleteCinemaHallCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<Unit> Handle(DeleteCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var cinemaHallToDelete = await _context.CinemaHalls
                .FirstOrDefaultAsync(x => x.Id == request.CinemaHallId && x.StatusId != 0, cancellationToken);

            if (cinemaHallToDelete == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            _context.CinemaHalls.Remove(cinemaHallToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        #endregion
    }
}
