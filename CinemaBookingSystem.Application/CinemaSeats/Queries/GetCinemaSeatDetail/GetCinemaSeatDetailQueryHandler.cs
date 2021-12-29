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

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeatDetail
{
    public class GetCinemaSeatDetailQueryHandler : IRequestHandler<GetCinemaSeatDetailQuery, CinemaSeatDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        public GetCinemaSeatDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CinemaSeatDetailVm> Handle(GetCinemaSeatDetailQuery request, CancellationToken cancellationToken)
        {
            var cinemaSeat = await _context.CinemaSeats.Where(x => x.Id == request.CinemaSeatId && x.StatusId != 0)
                .FirstOrDefaultAsync(cancellationToken);

            if (cinemaSeat == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }


            var cinemaSeatVm = _mapper.Map<CinemaSeatDetailVm>(cinemaSeat);

            return cinemaSeatVm;
        }
    }
}
