using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Queries.GetSeanceSeatDetail
{
    public class GetSeanceSeatDetailQuery : IRequest<SeanceSeatVm>
    {
        public int SeanceSeatId { get; set; }
    }
}
