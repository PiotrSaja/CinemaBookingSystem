using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.Enums;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Payment : AuditableEntity
    {
        public double Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string DiscountCoupon { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
