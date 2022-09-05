using MediatR;

namespace CinemaBookingSystem.Application.Movies.Commands.CreateMovieFromExternalApi
{
    public class CreateMovieFromExternalApiCommand : IRequest<int>
    {
        public string ImdbId { get; set; }
    }
}
