using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CinemaBookingSystem.Application.Common.Exceptions;
using CinemaBookingSystem.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Movies.Queries.GetUserMovieVote
{
    public class GetUserMovieVoteQueryHandler : IRequestHandler<GetUserMovieVoteQuery, UserMovieVm>
    {
        private readonly ICinemaDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        #region GetUserMovieVoteQueryHandler()
        public GetUserMovieVoteQueryHandler(ICinemaDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }
        #endregion

        #region Handle()
        public async Task<UserMovieVm> Handle(GetUserMovieVoteQuery request, CancellationToken cancellationToken)
        {
            var userMovie = await _context.UserMovieVotes.FirstOrDefaultAsync(x =>
                                x.MovieId == request.MovieId &&
                                x.UserId == _userService.Id, cancellationToken);

            if (userMovie == null)
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Not exists in database, check your id");

            var userMovieVm = _mapper.Map<UserMovieVm>(userMovie);

            return userMovieVm;
        }
        #endregion
    }
}
