using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesDaysToPremiere
{
    public class MoviesDetailVm
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public ICollection<MovieDetailDto> Items { get; set; }
    }
}
