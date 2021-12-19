using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovies
{
    public class MoviesVm
    {
        public ICollection<MoviesDto> Items { get; set; }
    }
}
