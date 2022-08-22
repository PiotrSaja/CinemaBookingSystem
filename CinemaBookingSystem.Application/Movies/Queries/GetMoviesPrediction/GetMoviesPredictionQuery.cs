using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Movies.Queries.GetMoviesPrediction;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesPrediction
{
    public class GetMoviesPredictionQuery : IRequest<MoviesDetailVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
