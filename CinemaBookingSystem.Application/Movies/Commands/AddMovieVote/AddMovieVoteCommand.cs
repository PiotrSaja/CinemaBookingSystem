using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.AddMovieVote
{
    public class AddMovieVoteCommand : IRequest<int>
    {
        public int MovieId { get; set; }
        public double Vote { get; set; }
    }
}
