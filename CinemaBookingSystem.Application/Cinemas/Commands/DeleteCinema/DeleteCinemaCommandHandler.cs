using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Cinemas.Commands.DeleteCinema
{
    public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaCommand, Unit>
    {
        private readonly ICinemaDbContext _context;

        #region DeleteCinemaCommandHandler()
        public DeleteCinemaCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<Unit> Handle(DeleteCinemaCommand request, CancellationToken cancellationToken)
        {
            var cinemaToDelete = await _context.Cinemas
                .FirstOrDefaultAsync(x => x.Id == request.CinemaId && 
                                          x.StatusId != 0, cancellationToken);

            if (cinemaToDelete == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            _context.Cinemas.Remove(cinemaToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        #endregion
    }
}
