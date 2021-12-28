using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.UpdateCinemaHall
{
    public class UpdateCinemaHallCommand : IRequest<int>
    {
        public int CinemaHallId { get; set; }
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public int CinemaId { get; set; }
    }
}
