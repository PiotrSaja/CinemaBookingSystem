using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatsForSeance
{
    public class GetSeanceSeatsForSeanceQuery : IRequest<SeanceSeatsVm>
    {
        public int SeanceId { get; set; }
    }
}
