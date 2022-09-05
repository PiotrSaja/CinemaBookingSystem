using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Commands.AddMovieVote
{
    public class AddMovieVoteCommandHandler : IRequestHandler<AddMovieVoteCommand, int>
    {
        private readonly ICinemaDbContext _context;
        private readonly IUserService _userService;

        #region AddMovieVoteCommandHandler()
        public AddMovieVoteCommandHandler(ICinemaDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        #endregion

        #region Handle()
        public async Task<int> Handle(AddMovieVoteCommand request, CancellationToken cancellationToken)
        {
            var movie = await _context.Movies
                .FirstOrDefaultAsync(x => x.Id == request.MovieId, cancellationToken);

            if (movie == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            var userMovie =
                await _context.UserMovieVotes.FirstOrDefaultAsync(x =>
                    x.MovieId == request.MovieId && x.UserId == _userService.Id, cancellationToken);

            if(userMovie != null)
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "You cannot add vote to the current movie");

            var userMovieVote = new UserMovieVote()
            {
                MovieId = movie.Id,
                UserId = _userService.Id,
                Vote = request.Vote
            };

            await _context.UserMovieVotes.AddAsync(userMovieVote, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return userMovieVote.Id;
        }
        #endregion
    }
}
