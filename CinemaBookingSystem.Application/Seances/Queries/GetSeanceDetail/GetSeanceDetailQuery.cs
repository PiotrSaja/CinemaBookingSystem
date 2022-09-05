using MediatR;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeanceDetail
{
    public class GetSeanceDetailQuery : IRequest<SeanceDetailVm>
    {
        public int SeanceId { get; set; }
    }
}
