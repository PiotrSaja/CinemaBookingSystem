using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Movie : AuditableEntity
    {
        public string Name { get; set; }
    }
}
