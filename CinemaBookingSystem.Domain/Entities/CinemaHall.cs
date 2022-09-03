using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class CinemaHall : AuditableEntity
    {
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public ICollection<CinemaSeat> CinemaSeats { get; set; }
    }
}
