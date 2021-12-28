using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.DeleteCinemaHall
{
    public class DeleteCinemaHallCommand : IRequest
    {
        public int CinemaHallId { get; set; }
    }
}
