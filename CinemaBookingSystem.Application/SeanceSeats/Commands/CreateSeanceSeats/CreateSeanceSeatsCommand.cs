using System.Collections.Generic;
using MediatR;

namespace CinemaBookingSystem.Application.SeanceSeats.Commands.CreateSeanceSeats
{
    public class CreateSeanceSeatsCommand : IRequest<bool>
    {
        public int SeanceId { get; set; }
        public List<SeanceSeatsModel> SeanceSeats { get; set; }
    }

    #region SeanceSeatsModel()
    public class SeanceSeatsModel
    {
        public double Price { get; set; }
        public int CinemaSeatId { get; set; }
    }
    #endregion
}
