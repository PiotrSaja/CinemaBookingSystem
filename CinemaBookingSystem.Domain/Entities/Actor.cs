using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.ValueObjects;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Actor : AuditableEntity
    {
        public PersonalName ActorName { get; set; }
        public ICollection<Movie> Movies { get; set; }
    } 
}
