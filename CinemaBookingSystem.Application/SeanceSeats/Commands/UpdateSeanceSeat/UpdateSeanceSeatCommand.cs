using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.UpdateSeanceSeat
{
    public class UpdateSeanceSeatCommand : IRequest<int>
    {
        public int SeanceSeatId { get; set; }
        public double Price { get; set; }
        public int SeanceId { get; set; }
        public int CinemaSeatId { get; set; }
        public bool SeatStatus { get; set; }
    }
}
