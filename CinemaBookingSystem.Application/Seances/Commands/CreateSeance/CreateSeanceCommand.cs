using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.Seances.Commands.CreateSeance
{
    public class CreateSeanceCommand : IRequest<int>
    {
        public DateTime Date { get; set; }
        public SeanceType SeanceType { get; set; }
        public int MovieId { get; set; }
        public int CinemaHallId { get; set; }
    }
}
