using MediatR;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeances
{
    public class GetSeancesQuery : IRequest<SeancesVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
    }
}
