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

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.UpdateCinemaHall
{
    public class UpdateCinemaHallCommandHandler : IRequestHandler<UpdateCinemaHallCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public UpdateCinemaHallCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateCinemaHallCommand request, CancellationToken cancellationToken)
        {
            var cinemaHallToUpdate = await _context.CinemaHalls.Where(x => x.Id == request.CinemaHallId)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinemaHallToUpdate == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }
            var cinema = await _context.Cinemas.Where(x => x.Id == request.CinemaId)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (cinema == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema in database, check your CinemaId");
            }

            cinemaHallToUpdate.Name = request.Name;
            cinemaHallToUpdate.TotalSeats = request.TotalSeats;
            cinemaHallToUpdate.CinemaId = request.CinemaId;
            cinemaHallToUpdate.NumberOfColumns = request.NumberOfColumns;
            cinemaHallToUpdate.NumberOfRows = request.NumberOfRows;

            _context.CinemaHalls.Update(cinemaHallToUpdate);
            await _context.SaveChangesAsync(cancellationToken);

            return cinemaHallToUpdate.Id;
        }
    }
}
