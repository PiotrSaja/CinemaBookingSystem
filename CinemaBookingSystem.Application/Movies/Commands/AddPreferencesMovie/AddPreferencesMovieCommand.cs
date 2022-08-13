using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.AddPreferencesMovie
{
    public class AddPreferencesMovieCommand : IRequest<int>
    {
        public List<int> MoviesIds { get; set; }
    }
}
