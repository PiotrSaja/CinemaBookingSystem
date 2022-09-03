using System;
using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.Seances.Commands.UpdateSeance
{
    public class UpdateSeanceCommand : IRequest<int>
    {
        public int SeanceId { get; set; }
        public DateTime Date { get; set; }
        public SeanceType SeanceType { get; set; }
        public int MovieId { get; set; }
        public int CinemaHallId { get; set; }
    }
}
