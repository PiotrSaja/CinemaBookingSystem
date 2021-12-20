using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMovies
{
    public class GetMoviesQuery : IRequest<MoviesVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
