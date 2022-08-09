using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeats
{
    public class CreateSeanceSeatsCommand : IRequest<bool>
    {
        public int SeanceId { get; set; }
        public List<SeanceSeatsModel> SeanceSeats { get; set; }
    }

    public class SeanceSeatsModel
    {
        public double Price { get; set; }
        public int CinemaSeatId { get; set; }
    }
}
