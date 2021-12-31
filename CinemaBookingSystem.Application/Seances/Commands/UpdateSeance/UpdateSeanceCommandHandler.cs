﻿using System;
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

namespace CinemaBookingSystem.Application.Seances.Commands.UpdateSeance
{
    public class UpdateSeanceCommandHandler : IRequestHandler<UpdateSeanceCommand, int>
    {
        private readonly ICinemaDbContext _context;

        public UpdateSeanceCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateSeanceCommand request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances.Where(x => x.Id == request.SeanceId).FirstOrDefaultAsync(cancellationToken);

            if (seance == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }
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

            seance.Date = request.Date;
            seance.SeanceType = request.SeanceType;
            seance.CinemaHallId = request.CinemaHallId;
            seance.MovieId = request.MovieId;

            _context.Seances.Update(seance);

            await _context.SaveChangesAsync(cancellationToken);

            return seance.Id;
        }
    }
}
