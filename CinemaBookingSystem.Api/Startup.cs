using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application;
using CinemaBookingSystem.Infrastructure;
using CinemaBookingSystem.Persistence;
using CinemaBookingSystem.Api.Extensions;
using CinemaBookingSystem.Api.Services;
using CinemaBookingSystem.Application.Common.Interfaces;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;

namespace CinemaBookingSystem.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddPersistence(Configuration);
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false
                    };
                });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //UserService DI
            services.TryAddScoped(typeof(IUserService), typeof(UserService));

            services.TryAddScoped(typeof(IUserVoteService), typeof(UserVoteService));

            //SeatLockingService
            services.TryAddSingleton<ISeatLockingService, SeatLockingService>();
            services.AddSingleton<IHostedService, SeatLockingHostedService>();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        AuthorizationCode = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:5001/connect/token"),
                            Scopes = new Dictionary<string, string>()
                            {
                                {"api", "Full access to API"},
                                {"user", "User information"},
                                {"openid", "OpenId"}
                            }
                        }
                    }
                });
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "CinemaBookingSystem.Api",
                    Version = "v1",
                    Description = "Engineering work",
                    Contact = new OpenApiContact()
                    {
                        Email = "piotreksaja99@gmail.com",
                        Name = "Piotr Saja",
                        Url = new Uri("https://github.com/PiotrSaja")
                    }
                });
            });
            services.AddHealthChecks();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api");
                });
            });
            services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage());

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJobManager, IUserVoteService userVoteService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CinemaBookingSystem v1");
                    c.OAuthClientId("swagger");
                    c.OAuth2RedirectUrl("https://localhost:44334/swagger/oauth2-redirect.html");
                    c.OAuthUsePkce();
                });
            }
            app.UseCors("AllowAll");

            app.UseHealthChecks("/hc");

            app.ConfigureCustomExceptionMiddleware();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHangfireDashboard();

            recurringJobManager.AddOrUpdate("Clustering for movie recommendation", () => userVoteService.Clustering(CancellationToken.None), "0 0 * * *");
        }
    }
}
