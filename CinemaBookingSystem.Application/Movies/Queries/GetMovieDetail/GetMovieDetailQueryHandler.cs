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

        #region GetMovieDetailQueryHandler()
        public GetMovieDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Handle()
        public async Task<MovieDetailVm> Handle(GetMovieDetailQuery request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies
                .Include(x=>x.Genres)
                .Include(x=>x.Director)
                .Include(x=>x.Actors)
                .FirstOrDefaultAsync(p => p.Id == request.MovieId, cancellationToken);

            var movieVm = _mapper.Map<MovieDetailVm>(movie);

            return movieVm;
        }
        #endregion
    }
}
