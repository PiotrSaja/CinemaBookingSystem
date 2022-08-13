using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovies
{
    public class MoviesVm
    {
        public int CurrentPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public string SearchString { get; set; }
        public ICollection<MoviesDto> Items { get; set; }
    }
}
