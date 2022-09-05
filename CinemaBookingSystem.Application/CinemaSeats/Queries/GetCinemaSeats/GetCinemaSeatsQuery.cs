using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeats
{
    public class GetCinemaSeatsQuery : IRequest<CinemaSeatsVm>
    {
        public int CinemaHallId { get; set; }
    }
}
