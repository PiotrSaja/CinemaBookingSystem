using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Commands.DeleteBooking
{
    public class DeleteBookingCommand : IRequest
    {
        public int BookingId { get; set; }
    }
}
