using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Common;

namespace CinemaBookingSystem.Domain.Entities
{
    public class SeanceSeat : AuditableEntity
    {
        public bool SeatStatus { get; set; }
        public double Price { get; set; }
        public int? BookingId { get; set; }
        public Booking Booking { get; set; }
        public int? ShowId { get; set; }
        public Seance Seance { get; set; }
        public int? CinemaSeatId { get; set; }
        public CinemaSeat CinemaSeat { get; set; }
    }
}
