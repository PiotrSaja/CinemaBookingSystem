using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeancesOfCurrentMovieOnGivenCinemaAndDay
{
    public class SeanceDetailVm
    {
        public ICollection<SeanceDto> Items { get; set; }
    }
}
