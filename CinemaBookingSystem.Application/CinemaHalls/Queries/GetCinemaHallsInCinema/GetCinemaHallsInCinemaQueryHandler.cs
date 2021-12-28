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

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallsInCinema
{
    public class GetCinemaHallsInCinemaQueryHandler : IRequestHandler<GetCinemaHallsInCinemaQuery, CinemaHallsVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetCinemaHallsInCinemaQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CinemaHallsVm> Handle(GetCinemaHallsInCinemaQuery request, CancellationToken cancellationToken)
        {
            var cinema = await _context.Cinemas.Where(x => x.Id == request.CinemaId && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinema == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "Not exists cinema in database, check your CinemaId");
            }
            var cinemaHalls = await _context.CinemaHalls.Where(x => x.StatusId != 0 && x.CinemaId == request.CinemaId)
                .ToListAsync(cancellationToken);

            if (cinemaHalls.Count == 0)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");
            }

            var cinemaHallsDto = _mapper.Map<List<CinemaHall>, List<CinemaHallDto>>(cinemaHalls);

            var cinemaHallsVm = new CinemaHallsVm()
            {
                Items = cinemaHallsDto
            };

            return cinemaHallsVm;
        }
    }
}
