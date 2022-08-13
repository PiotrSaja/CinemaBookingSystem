using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookingDetail
{
    public class GetBookingDetailQuery : IRequest<BookingDetailVm>
    {
        public int BookingId { get; set; }
    }
}
