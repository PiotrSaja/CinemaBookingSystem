using CinemaBookingSystem.Common.Mailer.Abstractions;
using CinemaBookingSystem.Common.Mailer.Configuration;
using CinemaBookingSystem.Common.Mailer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBookingSystem.Common
{
    public static class DependencyInjection
    {
        #region AddCommon()
        public static IServiceCollection AddCommon(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.AddTransient<IMailService, MailService>();

            return services;
        }
        #endregion
    }
}
