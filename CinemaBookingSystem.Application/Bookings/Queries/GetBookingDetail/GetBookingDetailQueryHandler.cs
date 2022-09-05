using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookingDetail
{
    public class GetBookingDetailQueryHandler : IRequestHandler<GetBookingDetailQuery, BookingDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        #region GetBookingDetailQueryHandler()
        public GetBookingDetailQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Handle()
        public async Task<BookingDetailVm> Handle(GetBookingDetailQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings.Where(x => x.Id == request.BookingId && x.StatusId != 0)
                .Include(x => x.Seance)
                .ThenInclude(x => x.Movie)
                .Include(x => x.SeanceSeats)
                .ThenInclude(x => x.CinemaSeat)
                .ThenInclude(x => x.CinemaHall)
                .ThenInclude(x => x.Cinema)
                .FirstOrDefaultAsync(cancellationToken);

            if (booking == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            var bookingsVm = _mapper.Map<BookingDetailVm>(booking);

            return bookingsVm;
        }
        #endregion
    }
}
