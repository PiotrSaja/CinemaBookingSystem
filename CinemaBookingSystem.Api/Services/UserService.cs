using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CinemaBookingSystem.Application.Common.Interfaces;
using IdentityModel;

namespace CinemaBookingSystem.Api.Services
{
    public class UserService : IUserService
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsAuthenticated { get; set; }

        #region UserService()
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            var id = httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtClaimTypes.Id);
            var email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
            var role = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);

            Id = id;
            Email = email;
            Role = role;

            IsAuthenticated = !string.IsNullOrEmpty(email);
        }
        #endregion
    }
}
