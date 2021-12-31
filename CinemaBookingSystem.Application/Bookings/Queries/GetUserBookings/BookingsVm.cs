using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class BookingsVm
    {
        public ICollection<BookingDto> Items { get; set; }
    }
}
