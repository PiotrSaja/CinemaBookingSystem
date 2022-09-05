using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesContentBasedPrediction
{
    public class GetMoviesContentBasedPredictionQuery : IRequest<MoviesDetailVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
