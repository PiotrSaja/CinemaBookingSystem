using System.Collections.Generic;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeancesOfCurrentMovieOnGivenCinemaAndDay
{
    public class SeanceDetailVm
    {
        public ICollection<SeanceDto> Items { get; set; }
    }
}
