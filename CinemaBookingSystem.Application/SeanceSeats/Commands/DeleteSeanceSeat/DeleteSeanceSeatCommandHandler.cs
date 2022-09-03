using System.Net;
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

        #region DeleteSeanceSeatCommandHandler()
        public DeleteSeanceSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<Unit> Handle(DeleteSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seanceSeatToDelete = await _context.SeanceSeats
                .FirstOrDefaultAsync(x => x.Id == request.SeanceSeatId && x.StatusId != 0, cancellationToken);

            if (seanceSeatToDelete == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            _context.SeanceSeats.Remove(seanceSeatToDelete);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        #endregion
    }
}
