using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Movies.Queries.GetPrefMovies
{
    public class MoviesDetailVm
    {
        public ICollection<MovieDetailDto> Items { get; set; }
    }
}
