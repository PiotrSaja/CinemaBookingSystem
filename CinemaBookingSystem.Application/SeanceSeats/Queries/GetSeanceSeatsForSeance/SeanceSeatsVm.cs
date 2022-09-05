using System.Collections.Generic;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatsForSeance
{
    public class SeanceSeatsVm
    {
        public ICollection<SeanceSeatDto> Items { get; set; }
    }
}
