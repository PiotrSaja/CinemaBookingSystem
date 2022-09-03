using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovieDetail
{
    public class GetMovieDetailQuery : IRequest<MovieDetailVm>
    {
        public int MovieId { get; set; }
    }
}
