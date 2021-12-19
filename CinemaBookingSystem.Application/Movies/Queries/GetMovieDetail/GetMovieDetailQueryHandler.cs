using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail
{
    public class GetMovieDetailQueryHandler : IRequestHandler<GetMovieDetailQuery, MovieDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private IMapper _mapper;

        public GetMovieDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MovieDetailVm> Handle(GetMovieDetailQuery request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies.Where(p => p.Id == request.MovieId && p.StatusId != 0)
                .Include(x=>x.Genres)
                .Include(x=>x.Director)
                .Include(x=>x.Actors)
                .FirstOrDefaultAsync(cancellationToken);


            var movieVm = _mapper.Map<MovieDetailVm>(movie);

            return movieVm;
        }
    }
}
