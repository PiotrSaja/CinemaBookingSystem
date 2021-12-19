using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovies
{
    public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery,MoviesVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetMoviesQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MoviesVm> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            var movies = await _context.Movies
                .Where(x => x.StatusId != 0)
                .Include(x => x.Genres)
                .ToListAsync(cancellationToken);

            if (movies == null)
            {
                throw new Exception();
            }
            var moviesDto = _mapper.Map<List<Movie>, List<MoviesDto>>(movies);

            var moviesVm = new MoviesVm()
            {
                Items =  moviesDto
            };

            return moviesVm;
        }
    }
}
