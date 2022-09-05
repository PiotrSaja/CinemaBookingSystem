using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Seances.Commands.DeleteSeance
{
    public class DeleteSeanceCommandHandler : IRequestHandler<DeleteSeanceCommand, Unit>
    {
        private readonly ICinemaDbContext _context;

        #region DeleteSeanceCommandHandler()
        public DeleteSeanceCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<Unit> Handle(DeleteSeanceCommand request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances.FirstOrDefaultAsync(x => x.Id == request.SeanceId && x.StatusId != 0, cancellationToken);

            if (seance == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            
            _context.Seances.Remove(seance);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
        #endregion
    }
}
