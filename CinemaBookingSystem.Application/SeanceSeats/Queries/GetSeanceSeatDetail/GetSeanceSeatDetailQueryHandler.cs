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

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatDetail
{
    public class GetSeanceSeatDetailQueryHandler : IRequestHandler<GetSeanceSeatDetailQuery, SeanceSeatVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetSeanceSeatDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SeanceSeatVm> Handle(GetSeanceSeatDetailQuery request, CancellationToken cancellationToken)
        {
            var seanceSeats = await _context.SeanceSeats
                .Where(x => x.StatusId != 0 && x.Id == request.SeanceSeatId)
                .Include(x => x.CinemaSeat)
                .Include(x => x.Seance)
                .Include(x => x.Booking)
                .FirstOrDefaultAsync(cancellationToken);
            var seanceSeatsVm = _mapper.Map<SeanceSeat, SeanceSeatVm>(seanceSeats);

            if (seanceSeats == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database or seanceSeatId is incorrect");
            }

            return seanceSeatsVm;
        }
    }
}
