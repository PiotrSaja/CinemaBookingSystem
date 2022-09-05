using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatDetail
{
    public class GetSeanceSeatDetailQuery : IRequest<SeanceSeatVm>
    {
        public int SeanceSeatId { get; set; }
    }
}
