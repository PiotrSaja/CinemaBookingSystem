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

namespace CinemaBookingSystem.Application.Seances.Commands.CreateSeance
{
    public class CreateSeanceCommandHandler : IRequestHandler<CreateSeanceCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public CreateSeanceCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateSeanceCommand request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies.Where(x => x.Id == request.MovieId)
                .FirstOrDefaultAsync(CancellationToken.None);
            var cinemaHall = await _context.CinemaHalls.Where(x => x.Id == request.CinemaHallId)
                .FirstOrDefaultAsync(CancellationToken.None);

            if (movie == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists movie in database, check your MovieId");
            }
            if (cinemaHall == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema hall in database, check your CinemaHallId");
            }
            var seance = new Seance()
            {
                Date = request.Date,
                SeanceType = request.SeanceType,
                CinemaHallId = request.CinemaHallId,
                MovieId = request.MovieId
            };

            _context.Seances.Add(seance);

            await _context.SaveChangesAsync(cancellationToken);

            return seance.Id;
        }
    }
}
