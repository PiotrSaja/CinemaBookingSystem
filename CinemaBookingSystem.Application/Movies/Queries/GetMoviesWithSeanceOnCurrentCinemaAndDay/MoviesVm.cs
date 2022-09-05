using System;
using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesWithSeanceOnCurrentCinemaAndDay
{
    public class MoviesVm
    {
        public int CinemaId { get; set; }
        public DateTime Date { get; set; }
        public List<MovieDto> Items { get; set; }
    }
}
