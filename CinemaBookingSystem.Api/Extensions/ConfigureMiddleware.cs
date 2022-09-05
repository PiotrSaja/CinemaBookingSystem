using Microsoft.AspNetCore.Builder;

namespace CinemaBookingSystem.Api.Extensions
{
    public static class ConfigureMiddleware
    {
        #region ConfigureCustomExceptionMiddleware()

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }

        #endregion
    }
}
