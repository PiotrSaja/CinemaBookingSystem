using System;

namespace CinemaBookingSystem.Application.Common.Models
{
    public class SeatLockingModel
    {
        public int SeanceSeatId { get; set; }
        public string UserId { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
