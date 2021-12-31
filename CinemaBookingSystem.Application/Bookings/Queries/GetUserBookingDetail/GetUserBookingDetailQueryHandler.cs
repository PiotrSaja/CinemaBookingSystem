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

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class GetUserBookingDetailQueryHandler : IRequestHandler<GetUserBookingDetailQuery, BookingDetailVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetUserBookingDetailQueryHandler(ICinemaDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<BookingDetailVm> Handle(GetUserBookingDetailQuery request, CancellationToken cancellationToken)
        {
            var booking = await _context.Bookings.Where(x => x.Id == request.BookingId &&
                                                             x.StatusId != 0 &&
                                                             x.UserId == _userService.Id)
                .Include(x => x.Seance)
                .ThenInclude(x => x.Movie)
                .Include(x => x.Seance)
                .ThenInclude(x => x.CinemaHall)
                .ThenInclude(x => x.Cinema)
                .Include(x => x.SeanceSeats)
                .ThenInclude(x => x.CinemaSeat)
                .FirstOrDefaultAsync(cancellationToken);


            if (booking == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");
            }

            var bookingDetailVm = _mapper.Map<BookingDetailVm>(booking);

            return bookingDetailVm;
        }
    }
}
