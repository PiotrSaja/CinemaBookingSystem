using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Domain.Entities
{
    public class UserRecommendationType : AuditableEntity
    {
    public string UserId { get; set; }
    public RecommendationType RecommendationType { get; set; }
    }
}
