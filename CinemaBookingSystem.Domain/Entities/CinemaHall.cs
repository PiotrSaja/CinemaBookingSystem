using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class CinemaHall : AuditableEntity
    {
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<CinemaSeat> CinemaSeats { get; set; }
    }
}
