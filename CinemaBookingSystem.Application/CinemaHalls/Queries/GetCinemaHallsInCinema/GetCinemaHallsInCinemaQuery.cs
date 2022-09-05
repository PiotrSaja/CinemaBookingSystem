using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallsInCinema
{
    public class GetCinemaHallsInCinemaQuery : IRequest<CinemaHallsVm>
    {
        public int CinemaId { get; set; }
    }
}
