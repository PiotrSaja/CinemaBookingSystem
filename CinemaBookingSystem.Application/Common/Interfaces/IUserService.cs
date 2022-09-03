namespace CinemaBookingSystem.Application.Common.Interfaces
{
    public interface IUserService
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
