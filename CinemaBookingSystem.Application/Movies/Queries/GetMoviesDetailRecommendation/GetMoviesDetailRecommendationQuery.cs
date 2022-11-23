using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesDetailRecommendation
{
    public class GetMoviesDetailRecommendationQuery : IRequest<MoviesDetailVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int SelectedMovieId { get; set; }
    }
}
