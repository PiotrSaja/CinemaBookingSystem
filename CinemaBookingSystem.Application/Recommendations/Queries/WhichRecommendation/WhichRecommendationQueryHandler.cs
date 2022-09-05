using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Recommendations.Queries.WhichRecommendation
{
    public class WhichRecommendationQueryHandler : IRequestHandler<WhichRecommendationQuery, RecommendationType>
    {
        private readonly ICinemaDbContext _context;
        private readonly IUserService _userService;

        #region WhichRecommendationQueryHandler()
        public WhichRecommendationQueryHandler(ICinemaDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        #endregion

        #region Handle()
        public async Task<RecommendationType> Handle(WhichRecommendationQuery request, CancellationToken cancellationToken)
        {
            var moviesPreferences = await _context.UserPreferencesMovies
                .Where(x => x.UserId == _userService.Id)
                .Select(x => x.MovieId)
                .ToListAsync(cancellationToken);

            var moviesVotes = await _context.UserMovieVotes
                .Where(x => x.UserId == _userService.Id)
                .ToListAsync(cancellationToken);

            if (moviesPreferences.Count > 0 && moviesVotes.Count <= 5)
                return RecommendationType.ContentBased;
            if (moviesVotes.Count > 5)
                return RecommendationType.KMeans;

            return RecommendationType.None;
        }
        #endregion
    }
}
