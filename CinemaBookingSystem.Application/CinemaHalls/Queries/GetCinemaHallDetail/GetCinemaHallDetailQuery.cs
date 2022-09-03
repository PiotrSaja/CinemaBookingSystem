using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallDetail
{
    public class GetCinemaHallDetailQuery : IRequest<CinemaHallDetailVm>
    {
        public int CinemaHallId { get; set; }
    }
}
