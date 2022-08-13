using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.DeleteCinemaSeat
{
    public class DeleteCinemaSeatCommand : IRequest
    {
        public int CinemaSeatId { get; set; }
    }
}
