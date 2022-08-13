using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.DeleteSeanceSeat
{
    public class DeleteSeanceSeatCommand : IRequest
    {
        public int SeanceSeatId { get; set; }
    }
}
