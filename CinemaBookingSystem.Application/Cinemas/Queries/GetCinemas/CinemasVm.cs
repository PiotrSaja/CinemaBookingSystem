using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Cinemas.Queries.GetCinemas
{
    public class CinemasVm
    {
        public ICollection<CinemaDto> Items { get; set; }
    }
}
