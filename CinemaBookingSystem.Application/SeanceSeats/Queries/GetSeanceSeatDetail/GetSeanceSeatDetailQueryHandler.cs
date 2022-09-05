using System.Net;
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

        #region GetSeanceSeatDetailQueryHandler()
        public GetSeanceSeatDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Handle()
        public async Task<SeanceSeatVm> Handle(GetSeanceSeatDetailQuery request, CancellationToken cancellationToken)
        {
            var seanceSeat = await _context.SeanceSeats
                .Include(x => x.CinemaSeat)
                .Include(x => x.Seance)
                .Include(x => x.Booking)
                .FirstOrDefaultAsync(x => x.StatusId != 0 && x.Id == request.SeanceSeatId, cancellationToken);

            if (seanceSeat == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database or seanceSeatId is incorrect");

            var seanceSeatsVm = _mapper.Map<SeanceSeat, SeanceSeatVm>(seanceSeat);

            return seanceSeatsVm;
        }
        #endregion
    }
}
