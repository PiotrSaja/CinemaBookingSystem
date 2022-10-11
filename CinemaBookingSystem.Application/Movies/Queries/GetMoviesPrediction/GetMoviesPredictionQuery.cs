using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesPrediction
{
    public class GetMoviesPredictionQuery : IRequest<MoviesDetailVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int SelectedMovieId { get; set; }
    }
}
