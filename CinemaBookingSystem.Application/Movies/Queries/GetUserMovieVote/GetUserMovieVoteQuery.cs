using MediatR;

namespace CinemaBookingSystem.Application.Movies.Queries.GetUserMovieVote
{
    public class GetUserMovieVoteQuery : IRequest<UserMovieVm>
    {
        public int MovieId { get; set; }
    }
}
