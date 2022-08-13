using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public DeleteSeanceCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteSeanceCommand request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances.Where(x => x.Id == request.SeanceId && x.StatusId != 0).FirstOrDefaultAsync(cancellationToken);

            if (seance == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }
            _context.Seances.Remove(seance);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
