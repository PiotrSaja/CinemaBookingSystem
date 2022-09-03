using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.DeleteMovie
{
    public class DeleteMovieCommand : IRequest
    {
        public int MovieId { get; set; }
    }
}
