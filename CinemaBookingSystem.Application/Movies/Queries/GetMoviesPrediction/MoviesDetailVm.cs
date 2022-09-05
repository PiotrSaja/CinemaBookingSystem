using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesPrediction
{
    public class MoviesDetailVm
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public ICollection<MovieDetailDto> Items { get; set; }
    }
}
