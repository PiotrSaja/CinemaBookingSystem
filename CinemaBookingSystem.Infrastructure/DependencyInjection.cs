using System;
using System.Net.Http;
using CinemaBookingSystem.Application.Common.Interfaces;
using CinemaBookingSystem.Infrastructure.ExternalAPI.OMDB;
using CinemaBookingSystem.Infrastructure.ExternalAPI.TMDB;
using CinemaBookingSystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBookingSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpClient("OmdbClient", options =>
            {
                options.BaseAddress = new Uri("http://www.omdbapi.com");
                options.Timeout = new TimeSpan(0, 0, 15);
                options.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }).ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler());
            services.AddScoped<IOmdbClient, OmdbClient>();

            services.AddHttpClient("TmdbClient", options =>
            {
                options.BaseAddress = new Uri("https://api.themoviedb.org/3");
                options.Timeout = new TimeSpan(0, 0, 15);
                options.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            }).ConfigurePrimaryHttpMessageHandler(sp => new HttpClientHandler());

            services.AddScoped<ITmdbClient, TmdbClient>();

            services.AddTransient<IDateTime, DateTimeService>();

            return services;
        }
    }
}
