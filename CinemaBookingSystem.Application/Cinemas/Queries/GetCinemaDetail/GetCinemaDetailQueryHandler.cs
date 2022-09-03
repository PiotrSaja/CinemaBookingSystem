using System.Net;
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

        #region GetCinemaDetailQueryHandler()
        public GetCinemaDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Handle()
        public async Task<CinemaDetailVm> Handle(GetCinemaDetailQuery request, CancellationToken cancellationToken)
        {
            var cinema = await _context.Cinemas
                .Include(x=>x.CinemaHalls)
                .FirstOrDefaultAsync(x => x.Id == request.CinemaId && x.StatusId != 0, cancellationToken);

            if (cinema == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            var cinemaDetailVm = _mapper.Map<CinemaDetailVm>(cinema);

            return cinemaDetailVm;
        }
        #endregion
    }
}
