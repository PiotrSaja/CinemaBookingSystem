using System.Collections.Generic;
using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.CreateCinemaSeats
{
    public class CreateCinemaSeatsCommand : IRequest<bool>
    {
        public int CinemaHallId { get; set; }
        public List<CinemaSeatsModel> CinemaSeats { get; set; }
    }

    #region CinemaSeatsModel()
    public class CinemaSeatsModel
    {
        public int SeatNumber { get; set; }
        public int Row { get; set; }
        public SeatType SeatType { get; set; }
        public int CinemaHallId { get; set; }
    }
    #endregion
}
