using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Extensions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookings
{
    public class GetBookingsQueryHandler : IRequestHandler<GetBookingsQuery, BookingsVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;

        #region GetBookingsQueryHandler()

        public GetBookingsQueryHandler(ICinemaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        #region Handle()
        public async Task<BookingsVm> Handle(GetBookingsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageSize < 1 && request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.UnprocessableEntity, "Page size and Page index can't be null or less than 1"); }
            if (request.PageSize < 1) { throw new HttpStatusCodeException(HttpStatusCode.UnprocessableEntity, "Page size can't be null or less than 1"); }
            if (request.PageIndex < 1) { throw new HttpStatusCodeException(HttpStatusCode.UnprocessableEntity, "Page index can't be null or less than 1"); }

            var bookings = await _context.Bookings
                .Where(x => x.StatusId != 0)
                .OrderBy(x => x.Created)
                .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);

            if (!String.IsNullOrEmpty(request.SearchString))
            {
                bookings = await _context.Bookings
                    .Where(x => x.StatusId != 0 &&
                                (x.PersonalName.FirstName.ToLower().Contains(request.SearchString) ||
                                 x.PersonalName.LastName.ToLower().Contains(request.SearchString) ||
                                 (x.PersonalName.FirstName + " " + x.PersonalName.LastName).ToLower().Contains(request.SearchString) ||
                                 (x.PersonalName.LastName + " " + x.PersonalName.FirstName).ToLower().Contains(request.SearchString)))
                    .OrderBy(p => p.Created)
                    .PaginateAsync(request.PageIndex, request.PageSize, cancellationToken);
            }

            if (bookings.Items.Count == 0)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists records in database");

            var bookingsDto = _mapper.Map<List<Booking>, List<BookingDto>>(bookings.Items.ToList());

            var bookingsVm = new BookingsVm()
            {
                CurrentPage = bookings.CurrentPage,
                TotalItems = bookings.TotalItems,
                TotalPages = bookings.TotalPages,
                SearchString = request.SearchString,
                Items = bookingsDto
            };

            return bookingsVm;
        }
        #endregion
    }
}
