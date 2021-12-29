using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatsForSeance
{
    public class SeanceSeatsVm
    {
        public ICollection<SeanceSeatDto> Items { get; set; }
    }
}
