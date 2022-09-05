using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.CreateCinemaHall
{
    public class CreateCinemaHallCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public int CinemaId { get; set; }
    }
}
