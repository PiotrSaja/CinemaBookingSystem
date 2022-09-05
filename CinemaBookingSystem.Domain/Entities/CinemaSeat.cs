using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Domain.Entities
{
    public class CinemaSeat : AuditableEntity
    {
        public int SeatNumber { get; set; }
        public int Row { get; set; }
        public SeatType SeatType { get; set; }
        public int CinemaHallId { get; set; }
        public CinemaHall CinemaHall { get; set; }
        public ICollection<SeanceSeat> SeanceSeats { get; set; }
    }
}
