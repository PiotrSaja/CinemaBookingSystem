using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetBookingDetail
{
    public class GetBookingDetailQuery : IRequest<BookingDetailVm>
    {
        public int BookingId { get; set; }
    }
}
