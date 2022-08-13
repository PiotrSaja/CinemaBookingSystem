using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

        public CreateSeanceSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateSeanceSeatCommand request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances.Where(x => x.Id == request.SeanceId)
                .FirstOrDefaultAsync(CancellationToken.None);
            var cinemaSeat = await _context.CinemaSeats.Where(x => x.Id == request.CinemaSeatId)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (seance == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists seance in database, check your seanceId");
            }
            if (cinemaSeat == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema seat in database, check your CinemaSeatId");
            }

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
    }
}
