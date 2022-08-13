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

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.UpdateCinemaSeat
{
    public class UpdateCinemaSeatCommandHandler : IRequestHandler<UpdateCinemaSeatCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public UpdateCinemaSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateCinemaSeatCommand request, CancellationToken cancellationToken)
        {
            var cinemaSeatToUpdate = await _context.CinemaSeats.Where(x => x.Id == request.CinemaSeatId)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinemaSeatToUpdate == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            var cinemaHall = await _context.Cinemas.Where(x => x.Id == request.CinemaHallId)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (cinemaHall == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema hall in database, check your CinemaHallId");
            }

            cinemaSeatToUpdate.SeatNumber = request.SeatNumber;
            cinemaSeatToUpdate.Row = request.Row;
            cinemaSeatToUpdate.SeatType = request.SeatType;
            cinemaSeatToUpdate.CinemaHallId = request.CinemaHallId;

            _context.CinemaSeats.Update(cinemaSeatToUpdate);

            await _context.SaveChangesAsync(cancellationToken);

            return cinemaSeatToUpdate.Id;
        }
    }
}
