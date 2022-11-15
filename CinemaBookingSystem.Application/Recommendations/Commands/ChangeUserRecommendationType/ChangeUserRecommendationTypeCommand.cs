using CinemaBookingSystem.Domain.Entities;
using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.Recommendations.Commands.ChangeUserRecommendationType
{
    public class ChangeUserRecommendationTypeCommand : IRequest<bool>
    {
        public RecommendationType RecommendationType { get; set; }
    }
}
