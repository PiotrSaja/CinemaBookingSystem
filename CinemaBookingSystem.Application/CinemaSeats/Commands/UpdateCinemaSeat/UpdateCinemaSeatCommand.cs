using CinemaBookingSystem.Domain.Enums;
using MediatR;

namespace CinemaBookingSystem.Application.CinemaSeats.Commands.UpdateCinemaSeat
{
    public class UpdateCinemaSeatCommand : IRequest<int>
    {
        public int CinemaSeatId { get; set; }
        public int SeatNumber { get; set; }
        public int Row { get; set; }
        public SeatType SeatType { get; set; }
        public int CinemaHallId { get; set; }
    }
}
