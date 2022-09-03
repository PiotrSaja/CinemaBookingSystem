using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class BookingsVm
    {
        public ICollection<BookingDto> Items { get; set; }
    }
}
