using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Enums;
using CinemaBookingSystem.Domain.ValueObjects;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Booking : AuditableEntity
    {
        public PersonalName PersonalName { get; set; }
        public int NumberOfSeats { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public int SeanceId { get; set; }
        public Seance Seance { get; set; }
        public ICollection<SeanceSeat> SeanceSeats { get; set; }
        public string UserId { get; set; }
    }
}
