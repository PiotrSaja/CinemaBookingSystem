using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.Recommendations.Queries.WhichRecommendation
{
    public class WhichRecommendationQuery : IRequest<RecommendationType>
    {
    }
}
