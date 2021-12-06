using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Genre : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
