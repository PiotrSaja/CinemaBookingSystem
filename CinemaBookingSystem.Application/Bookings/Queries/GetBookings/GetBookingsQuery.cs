using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookings
{
    public class GetBookingsQuery : IRequest<BookingsVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string SearchString { get; set; }
    }
}
