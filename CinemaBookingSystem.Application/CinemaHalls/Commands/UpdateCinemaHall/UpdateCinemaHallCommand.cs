using MediatR;

namespace CinemaBookingSystem.Application.CinemaHalls.Commands.UpdateCinemaHall
{
    public class UpdateCinemaHallCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public int CinemaId { get; set; }
    }
}
