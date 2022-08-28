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

namespace CinemaBookingSystem.Application.Cinemas.Queries.GetCinemaDetail
{
    public class GetCinemaDetailQueryHandler : IRequestHandler<GetCinemaDetailQuery, CinemaDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetCinemaDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CinemaDetailVm> Handle(GetCinemaDetailQuery request, CancellationToken cancellationToken)
        {
            var cinema = await _context.Cinemas.Where(x => x.Id == request.CinemaId && x.StatusId != 0)
                .Include(x=>x.CinemaHalls)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinema == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            var cinemaDetailVm = _mapper.Map<CinemaDetailVm>(cinema);

            return cinemaDetailVm;
        }
    }
}
