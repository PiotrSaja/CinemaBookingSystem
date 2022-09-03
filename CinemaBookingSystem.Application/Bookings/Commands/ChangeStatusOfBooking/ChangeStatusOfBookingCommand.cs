using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Commands.ChangeStatusOfBooking
{
    public class ChangeStatusOfBookingCommand : IRequest<int>
    {
        public int BookingId { get; set; }
        public BookingStatus Status { get; set; }
    }
}
