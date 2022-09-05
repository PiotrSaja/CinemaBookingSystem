using System.Net;
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

        #region CreateSeanceCommandHandler()
        public CreateSeanceCommandHandler(ICinemaDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(CreateSeanceCommand request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(x => x.Id == request.MovieId, cancellationToken);

            var cinemaHall = await _context.CinemaHalls
                .FirstOrDefaultAsync(x => x.Id == request.CinemaHallId, cancellationToken);

            if (movie == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists movie in database, check your MovieId");
            if (cinemaHall == null)
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema hall in database, check your CinemaHallId");
            
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
        #endregion
    }
}
