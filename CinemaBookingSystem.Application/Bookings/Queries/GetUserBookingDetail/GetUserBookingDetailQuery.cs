using MediatR;

namespace CinemaBookingSystem.Application.Bookings.Queries.GetUserBookingDetail
{
    public class GetUserBookingDetailQuery : IRequest<BookingDetailVm>
    {
        public int BookingId { get; set; }
    }
}
