using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class UserMovieVote : AuditableEntity
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string UserId { get; set; }
        public double Vote { get; set; }
    }
}
