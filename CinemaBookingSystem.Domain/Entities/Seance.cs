using System;
using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Seance : AuditableEntity
    {
        public DateTime Date { get; set; }
        public SeanceType SeanceType { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int CinemaHallId { get; set; }
        public CinemaHall CinemaHall { get; set; }
        public ICollection<SeanceSeat> SeanceSeats { get; set; }
    }
}
