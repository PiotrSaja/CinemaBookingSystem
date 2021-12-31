using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookings
{
    public class GetUserBookingsQuery : IRequest<BookingsVm>
    {
    }
}
