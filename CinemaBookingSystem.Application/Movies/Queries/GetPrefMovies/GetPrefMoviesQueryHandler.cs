using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Queries.GetPrefMovies
{
    public class GetPrefMoviesQueryHandler : IRequestHandler<GetPrefMoviesQuery, MoviesDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        #region GetPrefMoviesQueryHandler()
        public GetPrefMoviesQueryHandler(ICinemaDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }
        #endregion

        #region Handle()
        public async Task<MoviesDetailVm> Handle(GetPrefMoviesQuery request, CancellationToken cancellationToken)
        {
            var moviesPref = await _context.UserPreferencesMovies
                .Where(x => x.UserId == _userService.Id)
                .Select(x=>x.MovieId)
                .ToListAsync(cancellationToken);

            var movies = await _context.Movies
                .Where(x => x.StatusId != 0 &&
                            moviesPref.Contains(x.Id))
                .ToListAsync(cancellationToken);

            var moviesDto = _mapper.Map<List<Movie>, List<MovieDetailDto>>(movies);

            var moviesDetailVm = new MoviesDetailVm()
            {
                Items = moviesDto
            };

            return moviesDetailVm;
        }
        #endregion
    }
}
