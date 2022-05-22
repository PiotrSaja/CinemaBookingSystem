using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.Seances.Queries.GetSeancesOfCurrentMovieOnGivenCinemaAndDay
{
    public class GetSeancesOfCurrentMovieOnGivenCinemaAndDateQuery : IRequest<SeanceDetailVm>
    {
        public int CinemaId { get; set; }
        public DateTime Date { get; set; }
        public int MovieId { get; set; }
    }
}
