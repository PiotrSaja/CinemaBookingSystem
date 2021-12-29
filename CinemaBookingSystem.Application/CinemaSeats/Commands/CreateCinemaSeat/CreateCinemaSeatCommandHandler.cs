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

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.CreateCinemaSeat
{
    public class CreateCinemaSeatCommandHandler : IRequestHandler<CreateCinemaSeatCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public CreateCinemaSeatCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCinemaSeatCommand request, CancellationToken cancellationToken)
        {
            var cinemaHall = await _context.Cinemas.Where(x => x.Id == request.CinemaHallId)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (cinemaHall == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema hall in database, check your CinemaHallId");
            }
            var newCinemaSeat = new CinemaSeat()
            {
                SeatNumber = request.SeatNumber,
                Row = request.Row,
                SeatType = request.SeatType,
                CinemaHallId = request.CinemaHallId
            };
            _context.CinemaSeats.Add(newCinemaSeat);
            await _context.SaveChangesAsync(cancellationToken);

            return newCinemaSeat.Id;
        }
    }
}
