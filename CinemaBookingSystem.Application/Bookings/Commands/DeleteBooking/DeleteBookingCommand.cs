using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommand : IRequest
    {
        public int BookingId { get; set; }
    }
}
