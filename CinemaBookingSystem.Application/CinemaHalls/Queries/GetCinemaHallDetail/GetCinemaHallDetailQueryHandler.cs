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

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallDetail
{
    public class GetCinemaHallDetailQueryHandler : IRequestHandler<GetCinemaHallDetailQuery, CinemaHallDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetCinemaHallDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CinemaHallDetailVm> Handle(GetCinemaHallDetailQuery request, CancellationToken cancellationToken)
        {
            var cinemaHall = await _context.CinemaHalls
                .Where(x => x.Id == request.CinemaHallId && x.StatusId != 0)
                .Include(x=>x.Cinema)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinemaHall == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            var cinemaHallVm = _mapper.Map<CinemaHallDetailVm>(cinemaHall);

            return cinemaHallVm;
        }
    }
}
