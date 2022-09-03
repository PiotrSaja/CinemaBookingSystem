using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeancesOfCurrentMovieOnGivenCinemaAndDay
{
    public class GetSeancesOfCurrentMovieOnGivenCinemaAndDateQueryHandler : IRequestHandler<GetSeancesOfCurrentMovieOnGivenCinemaAndDateQuery, SeanceDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        #region GetSeancesOfCurrentMovieOnGivenCinemaAndDateQueryHandler()
        public GetSeancesOfCurrentMovieOnGivenCinemaAndDateQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Handle()
        public async Task<SeanceDetailVm> Handle(GetSeancesOfCurrentMovieOnGivenCinemaAndDateQuery request, CancellationToken cancellationToken)
        {
            var seances = await _context.Seances.Where(x => x.StatusId != 0 &&
                                                            x.CinemaHall.CinemaId == request.CinemaId && 
                                                            x.Date.Date == request.Date.Date && 
                                                            x.MovieId == request.MovieId)
                .Include(x => x.Movie)
                .Include(x => x.CinemaHall)
                .ToListAsync(cancellationToken);

            var seanceDto = _mapper.Map<List<Seance>, List<SeanceDto>>(seances.ToList());

            var seanceVm = new SeanceDetailVm()
            {
                Items = seanceDto
            };

            return seanceVm;
        }
        #endregion
    }
}
