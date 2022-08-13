using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class GetUserBookingDetailQuery : IRequest<BookingDetailVm>
    {
        public int BookingId { get; set; }
    }
}
