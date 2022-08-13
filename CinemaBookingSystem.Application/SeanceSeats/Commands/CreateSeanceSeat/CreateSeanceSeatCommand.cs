using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeat
{
    public class CreateSeanceSeatCommand : IRequest<int>
    {
        public double Price { get; set; }
        public int SeanceId { get; set; }
        public int CinemaSeatId { get; set; }
    }
}
