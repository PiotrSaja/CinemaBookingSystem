using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Movies.Queries.GetPrefMovies
{
    public class MoviesDetailVm
    {
        public ICollection<MovieDetailDto> Items { get; set; }
    }
}
