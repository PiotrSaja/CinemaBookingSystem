// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace IdentityServer
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    #region Users
                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var alice = userMgr.FindByNameAsync("alice").Result;

                    var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    var user = roleMgr.FindByNameAsync("User").Result;
                    var admin = roleMgr.FindByNameAsync("Administrator").Result;

                    if (user == null)
                    {
                        user = new IdentityRole()
                        {
                            Name = "User"
                        };
                        Log.Debug("Role user created");
                        _ = roleMgr.CreateAsync(user).Result;
                    }


                    if (admin == null)
                    {
                        admin = new IdentityRole()
                        {
                            Name = "Administrator"
                        };
                        Log.Debug("Role admin created");
                        _ = roleMgr.CreateAsync(admin).Result;
                    }

                    if (alice == null)
                    {
                        alice = new ApplicationUser
                        {
                            UserName = "alice",
                            Email = "AliceSmith@email.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(alice, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("alice created");
                    }
                    else
                    {
                        Log.Debug("alice already exists");
                    }


                    var bob = userMgr.FindByNameAsync("bob").Result;

                    if (bob == null)
                    {
                        bob = new ApplicationUser
                        {
                            UserName = "bob",
                            Email = "BobSmith@email.com",
                            EmailConfirmed = true
                        };
                        var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(bob, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "Bob Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Bob"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                            new Claim("location", "somewhere")
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("bob created");
                    }
                    else
                    {
                        Log.Debug("bob already exists");
                    }

                    for (var i = 1; i <= 105; i++)
                    {
                        var applicationUser = userMgr.FindByNameAsync($"user{i}").Result;
                        if (applicationUser == null)
                        {
                            applicationUser = new ApplicationUser
                            {
                                UserName = $"user{i}",
                                Email = $"user{i}@example.com",
                                EmailConfirmed = true,
                            };
                            var result = userMgr.CreateAsync(applicationUser, "Pass123$").Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = userMgr.AddClaimsAsync(applicationUser, new Claim[]
                            {
                                new Claim(JwtClaimTypes.Name, $"User{i} User{i}"),
                                new Claim(JwtClaimTypes.GivenName, $"User{i}"),
                                new Claim(JwtClaimTypes.FamilyName, $"User{i}"),
                            }).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            Log.Debug($"user{i} created");
                        }
                        else
                        {
                            Log.Debug($"user{i} already exists");
                        }

                        if (!userMgr.IsInRoleAsync(applicationUser, user.Name).Result)
                        {
                            Log.Debug($"Adding user{i} to user role");
                            _ = userMgr.AddToRoleAsync(applicationUser, user.Name).Result;
                        }
                    }

                    #endregion

                    #region Roles

                    if (!userMgr.IsInRoleAsync(alice, user.Name).Result)
                    {
                        Log.Debug("Adding alice to user role");
                        _ = userMgr.AddToRoleAsync(alice, user.Name).Result;
                    }
                    if (!userMgr.IsInRoleAsync(bob, admin.Name).Result)
                    {
                        Log.Debug("Adding bob to administrator role");
                        _ = userMgr.AddToRoleAsync(bob, admin.Name).Result;
                    }

                    #endregion
                }
            }
        }
    }
}
