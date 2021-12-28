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

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.CreateCinemaHall
{
    public class CreateCinemaHallCommandHandler : IRequestHandler<CreateCinemaHallCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public CreateCinemaHallCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var cinemaFk = await _context.Cinemas.Where(x => x.Id == request.CinemaId)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (cinemaFk == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema in database, check your CinemaId");
            }
            var cinemaHall = new CinemaHall()
            {
                Name = request.Name,
                TotalSeats = request.TotalSeats,
                CinemaId = request.CinemaId
            };

            _context.CinemaHalls.Add(cinemaHall);

            await _context.SaveChangesAsync(cancellationToken);

            return cinemaHall.Id;
        }
    }
}
