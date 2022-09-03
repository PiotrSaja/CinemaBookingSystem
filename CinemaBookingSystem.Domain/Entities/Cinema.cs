using System.Collections.Generic;
using CinemaBookingSystem.Domain.Common;
using CinemaBookingSystem.Domain.ValueObjects;

namespace CinemaBookingSystem.Domain.Entities
{
    public class Cinema : AuditableEntity
    {
        public string Name { get; set; }
        public int TotalCinemaHalls { get; set; }
        public Address Address { get; set; }
        public string ImagePath { get; set; }
        public ICollection<CinemaHall> CinemaHalls { get; set; }
    }
}
