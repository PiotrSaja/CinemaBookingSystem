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

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, BookingsVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetUserBookingsQueryHandler(ICinemaDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<BookingsVm> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _context.Bookings
                .Where(x => x.StatusId != 0
                            && x.UserId == _userService.Id)
                .OrderBy(x => x.Created)
                .Include(x => x.Seance)
                .ThenInclude(x => x.Movie)
                .Include(x => x.Seance)
                .ThenInclude(x => x.CinemaHall)
                .ThenInclude(x => x.Cinema)
                .ToListAsync(cancellationToken);

            if (bookings.Count == 0)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");
            }

            var bookingsDto = _mapper.Map<List<Booking>, List<BookingDto>>(bookings);

            var getUserBookingsVm = new BookingsVm()
            {
                Items = bookingsDto
            };

            return getUserBookingsVm;
        }
    }
}
