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
            var userRecommendationType = await _context.UserRecommendationTypes
                .FirstOrDefaultAsync(x => x.UserId == _userService.Id, cancellationToken);

            if (userRecommendationType == null)
                return RecommendationType.None;
            if (userRecommendationType.RecommendationType == RecommendationType.ContentBased)
                return RecommendationType.ContentBased;
            if (userRecommendationType.RecommendationType == RecommendationType.KMeans)
                return RecommendationType.KMeans;

            return RecommendationType.None;
        }
        #endregion
    }
}
