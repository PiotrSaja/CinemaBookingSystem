using System.Collections.Generic;

namespace CinemaBookingSystem.Application.CinemaHalls.Queries.GetCinemaHallsInCinema
{
    public class CinemaHallsVm
    {
        public ICollection<CinemaHallDto> Items { get; set; }
    }
}
