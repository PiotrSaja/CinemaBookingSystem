using System.Collections.Generic;
using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.AddPreferencesMovie
{
    public class AddPreferencesMovieCommand : IRequest<int>
    {
        public List<int> MoviesIds { get; set; }
    }
}
