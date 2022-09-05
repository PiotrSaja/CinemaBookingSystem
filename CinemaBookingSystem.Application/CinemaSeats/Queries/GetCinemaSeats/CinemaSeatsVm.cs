using System.Collections.Generic;

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeats
{
    public class CinemaSeatsVm
    {
        public ICollection<CinemaSeatDto> Items { get; set; }
    }
}
