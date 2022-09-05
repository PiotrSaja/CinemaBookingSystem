using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatsForSeance
{
    public class GetSeanceSeatsForSeanceQuery : IRequest<SeanceSeatsVm>
    {
        public int SeanceId { get; set; }
    }
}
