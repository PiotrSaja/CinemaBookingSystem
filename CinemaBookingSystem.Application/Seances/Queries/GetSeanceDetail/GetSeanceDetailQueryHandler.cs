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
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class GetSeanceDetailQueryHandler : IRequestHandler<GetSeanceDetailQuery, SeanceDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetSeanceDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SeanceDetailVm> Handle(GetSeanceDetailQuery request, CancellationToken cancellationToken)
        {
            var seance = await _context.Seances
                .Where(x => x.Id == request.SeanceId && x.StatusId != 0)
                .Include(x => x.Movie)
                .ThenInclude(x=>x.Genres)
                .Include(x => x.CinemaHall)
                .ThenInclude(x=>x.Cinema)
                .FirstOrDefaultAsync(cancellationToken);

            if (seance == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database or seanceId is incorrect");
            }

            var seanceVm = _mapper.Map<SeanceDetailVm>(seance);

            return seanceVm;
        }
    }
}
