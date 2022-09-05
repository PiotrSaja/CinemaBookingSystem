using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeatDetail
{
    public class GetCinemaSeatDetailQuery : IRequest<CinemaSeatDetailVm>
    {
        public int CinemaSeatId { get; set; }
    }
}
