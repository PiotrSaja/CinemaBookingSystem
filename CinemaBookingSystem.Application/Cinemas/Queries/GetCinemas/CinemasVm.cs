using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBookingSystem.Application.Cinemas.Queries.GetCinemas
{
    public class CinemasVm
    {
        public ICollection<CinemaDto> Items { get; set; }
    }
}
