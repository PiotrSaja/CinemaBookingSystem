using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingSystem.Application.Recommendations.Commands.ChangeUserRecommendationType
{
    public class ChangeUserRecommendationTypeCommandHandler : IRequestHandler<ChangeUserRecommendationTypeCommand, bool>
    {
        private readonly ICinemaDbContext _context;
        private readonly IUserService _userService;

        #region ChangeUserRecommendationTypeCommandHandler()
        public ChangeUserRecommendationTypeCommandHandler(ICinemaDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }
        #endregion

        #region Handle()
        public async Task<bool> Handle(ChangeUserRecommendationTypeCommand request, CancellationToken cancellationToken)
        {
            var moviesPreferences = await _context.UserPreferencesMovies
                .Where(x => x.UserId == _userService.Id && x.StatusId != 0)
                .Select(x => x.MovieId)
                .ToListAsync(cancellationToken);

            var moviesVotes = await _context.UserMovieVotes
                .Where(x => x.UserId == _userService.Id)
                .ToListAsync(cancellationToken);

            var clusterSetForUser = await _context.UserClusters.FirstOrDefaultAsync(x => x.UserId == _userService.Id, cancellationToken);

            if (moviesPreferences.Count == 0 && request.RecommendationType == RecommendationType.ContentBased)
                throw new Exception("Can't change recommendation type to content based");

            if (moviesVotes.Count <= 5 && clusterSetForUser == null && request.RecommendationType == RecommendationType.KMeans)
                throw new Exception("Can't change recommendation type to collaborative filtering");

            var userRecommendation = await _context.UserRecommendationTypes.FirstOrDefaultAsync(x => x.UserId == _userService.Id,
                cancellationToken);

            if (userRecommendation == null)
            {
                _context.UserRecommendationTypes.Add(new UserRecommendationType()
                {
                    UserId = _userService.Id,
                    RecommendationType = request.RecommendationType
                });
            }
            else
            {
                userRecommendation.RecommendationType = request.RecommendationType;
                _context.UserRecommendationTypes.Update(userRecommendation);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
        #endregion
    }
}
