using MediatR;

namespace CinemaBookingSystem.Application.Cinemas.Commands.DeleteCinema
{
    public class DeleteCinemaCommand : IRequest
    {
        public int CinemaId { get; set; }
    }
}
