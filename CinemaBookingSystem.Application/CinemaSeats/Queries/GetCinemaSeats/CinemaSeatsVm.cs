using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.CinemaSeats.Queries.GetCinemaSeats
{
    public class CinemaSeatsVm
    {
        public ICollection<CinemaSeatDto> Items { get; set; }
    }
}
