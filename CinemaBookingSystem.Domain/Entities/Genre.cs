using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Genre : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Movie> Movies { get; set;}
    }
}
