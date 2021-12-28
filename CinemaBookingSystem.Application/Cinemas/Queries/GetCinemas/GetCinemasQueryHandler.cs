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

namespace CinemaBookingSystem.Application.Cinemas.Queries.GetCinemas
{
    public class GetCinemasQueryHandler : IRequestHandler<GetCinemasQuery, CinemasVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetCinemasQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CinemasVm> Handle(GetCinemasQuery request, CancellationToken cancellationToken)
        {
            var cinemasList = await _context.Cinemas.Where(x => x.StatusId != 0)
                .ToListAsync(cancellationToken);

            if (cinemasList == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");
            }

            var cinemasDto = _mapper.Map<List<Cinema>, List<CinemaDto>>(cinemasList);

            var cinemasVm = new CinemasVm()
            {
                Items = cinemasDto
            };

            return cinemasVm;
        }
    }
}
