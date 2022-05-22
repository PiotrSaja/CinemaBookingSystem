using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
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

        public GetSeancesOfCurrentMovieOnGivenCinemaAndDateQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SeanceDetailVm> Handle(GetSeancesOfCurrentMovieOnGivenCinemaAndDateQuery request, CancellationToken cancellationToken)
        {
            var seances = await _context.Seances.Where(x => x.StatusId != 0
                                                        && x.CinemaHall.CinemaId == request.CinemaId
                                                        && x.Date.Date == request.Date.Date
                                                        && x.MovieId == request.MovieId)
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
    }
}
