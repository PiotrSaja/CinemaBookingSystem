using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class UserCluster : AuditableEntity
    {
        public string UserId { get; set; }
        public int ClusterNumber { get; set; }
    }
}
