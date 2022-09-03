using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class UserCluster : AuditableEntity
    {
        public string UserId { get; set; }
        public int ClusterNumber { get; set; }
    }
}
