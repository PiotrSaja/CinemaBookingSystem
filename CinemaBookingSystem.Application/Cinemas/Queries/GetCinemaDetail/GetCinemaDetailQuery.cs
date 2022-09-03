using MediatR;

namespace CinemaBookingSystem.Application.Cinemas.Queries.GetCinemaDetail
{
    public class GetCinemaDetailQuery : IRequest<CinemaDetailVm>
    {
        public int CinemaId { get; set; }
    }
}
