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

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatsForSeance
{
    public class GetSeanceSeatsForSeanceQueryHandler : IRequestHandler<GetSeanceSeatsForSeanceQuery, SeanceSeatsVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetSeanceSeatsForSeanceQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SeanceSeatsVm> Handle(GetSeanceSeatsForSeanceQuery request, CancellationToken cancellationToken)
        {
            var seanceSeats = await _context.SeanceSeats
                .Where(x => x.StatusId != 0
                            && x.SeanceId == request.SeanceId)
                .Include(x => x.CinemaSeat)
                .ToListAsync(cancellationToken);

            if (seanceSeats == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database or seanceId is incorrect");
            }
            var seanceSeatsDto = _mapper.Map<List<SeanceSeat>, List<SeanceSeatDto>>(seanceSeats);

            var seanceSeatsVm = new SeanceSeatsVm()
            {
                Items = seanceSeatsDto
            };

            return seanceSeatsVm;
        }
    }
}
