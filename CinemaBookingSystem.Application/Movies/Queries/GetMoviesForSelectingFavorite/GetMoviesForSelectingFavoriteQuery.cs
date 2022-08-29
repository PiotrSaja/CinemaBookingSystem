using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Movies.Queries.GetMovies;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetMoviesForSelectingFavorite
{
    public class GetMoviesForSelectingFavoriteQuery : IRequest<MoviesVm>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string SearchString { get; set; }
        public int? GenreId { get; set; }
    }
}
