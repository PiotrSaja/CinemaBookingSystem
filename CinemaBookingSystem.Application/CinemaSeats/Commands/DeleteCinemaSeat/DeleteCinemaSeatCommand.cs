using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.DeleteCinemaSeat
{
    public class DeleteCinemaSeatCommand : IRequest
    {
        public int CinemaSeatId { get; set; }
    }
}
