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

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeats
{
    public class GetCinemaSeatsQueryHandler : IRequestHandler<GetCinemaSeatsQuery, CinemaSeatsVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetCinemaSeatsQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CinemaSeatsVm> Handle(GetCinemaSeatsQuery request, CancellationToken cancellationToken)
        {
            var cinemaSeats = await _context.CinemaSeats.Where(x => x.StatusId != 0 && x.CinemaHallId == request.CinemaHallId).ToListAsync(cancellationToken);

            if (cinemaSeats == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");
            }

            var cinemaSeatsDto = _mapper.Map<List<CinemaSeat>, List<CinemaSeatDto>>(cinemaSeats);

            var cinemaSeatsVm = new CinemaSeatsVm()
            {
                Items = cinemaSeatsDto
            };

            return cinemaSeatsVm;
        }
    }
}
