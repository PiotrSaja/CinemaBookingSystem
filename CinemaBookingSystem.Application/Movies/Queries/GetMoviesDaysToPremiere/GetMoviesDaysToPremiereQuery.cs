using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesDaysToPremiere
{
    public class GetMoviesDaysToPremiereQuery : IRequest<MoviesDetailVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int DaysToPremiere { get; set; }
    }
}
