using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.DeleteCinemaHall
{
    public class DeleteCinemaHallCommand : IRequest
    {
        public int CinemaHallId { get; set; }
    }
}
