using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.DeleteSeanceSeat
{
    public class DeleteSeanceSeatCommand : IRequest
    {
        public int SeanceSeatId { get; set; }
    }
}
