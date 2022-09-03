using MediatR;

namespace CinemaBookingSystem.Application.Seances.Commands.DeleteSeance
{
    public class DeleteSeanceCommand : IRequest
    {
        public int SeanceId { get; set; }
    }
}
