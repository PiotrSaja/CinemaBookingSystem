using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesWithSeanceOnCurrentCinemaAndDay
{
    public class GetMoviesWithSeanceOnCurrentCinemaAndDayQueryHandler : IRequestHandler<GetMoviesWithSeanceOnCurrentCinemaAndDayQuery, MoviesVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        #region GetMoviesWithSeanceOnCurrentCinemaAndDayQueryHandler()
        public GetMoviesWithSeanceOnCurrentCinemaAndDayQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Handle()
        public async Task<MoviesVm> Handle(GetMoviesWithSeanceOnCurrentCinemaAndDayQuery request, CancellationToken cancellationToken)
        {
            var movies = await _context.Movies
                .Include(x => x.Seances
                    .Where(x => x.Date.Date == request.Date.Date &&
                                x.CinemaHall.CinemaId == request.CinemaId &&
                                x.StatusId != 0)
                    .OrderBy(x=>x.Date))
                .ToListAsync(cancellationToken);

            var moviesWithSeances = movies
                .Where(x => x.Seances.Count > 0).ToList();

            var moviesDto = _mapper.Map<List<Movie>, List<MovieDto>>(moviesWithSeances);

            var moviesVm = new MoviesVm()
            {
                CinemaId = request.CinemaId,
                Date = request.Date,
                Items = moviesDto
            };

            return moviesVm;
        }
        #endregion
    }
}
