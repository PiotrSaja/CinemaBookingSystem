using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovies
{
    public class GetMoviesQuery : IRequest<MoviesVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string SearchString { get; set; }
        public int? GenreId { get; set; }
    }
}
