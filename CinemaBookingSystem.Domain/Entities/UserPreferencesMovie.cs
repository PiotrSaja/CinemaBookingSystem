using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class UserPreferencesMovie : AuditableEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string UserId { get; set; }
    }
}
