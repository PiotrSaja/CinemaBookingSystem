using Microsoft.AspNetCore.Builder;

namespace CinemaBookingSystem.Api.Extensions
{
    public static class ConfigureMiddleware
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
