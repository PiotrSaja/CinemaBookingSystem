using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBookingSystem.Persistence
{
    public static class DependencyInjection
    {
        #region AddPersistence()
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CinemaDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("CinemaDatabase")));

            services.AddScoped<ICinemaDbContext, CinemaDbContext>();
            return services;
        }
        #endregion
    }
}
