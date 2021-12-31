using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookings
{
    public class BookingsVm
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }

        public ICollection<BookingDto> Items { get; set; }
    }
}
