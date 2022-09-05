using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.ValueObjects;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Director : AuditableEntity
    {
        public PersonalName DirectorName { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
