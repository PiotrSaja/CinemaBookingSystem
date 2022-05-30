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

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var alice = userMgr.FindByNameAsync("alice").Result;

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

                    var user1 = userMgr.FindByNameAsync("user1").Result;
                    if (user1 == null)
                    {
                        user1 = new ApplicationUser
                        {
                            UserName = "user1",
                            Email = "user1@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user1, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user1, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User1 User1"),
                            new Claim(JwtClaimTypes.GivenName, "User1"),
                            new Claim(JwtClaimTypes.FamilyName, "User1"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user1 created");
                    }
                    else
                    {
                        Log.Debug("user1 already exists");
                    }

                    var user2 = userMgr.FindByNameAsync("user2").Result;
                    if (user2 == null)
                    {
                        user2 = new ApplicationUser
                        {
                            UserName = "user2",
                            Email = "user2@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user2, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user2, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User2 User2"),
                            new Claim(JwtClaimTypes.GivenName, "User2"),
                            new Claim(JwtClaimTypes.FamilyName, "User2"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user2 created");
                    }
                    else
                    {
                        Log.Debug("user2 already exists");
                    }

                    var user3 = userMgr.FindByNameAsync("user3").Result;
                    if (user3 == null)
                    {
                        user3 = new ApplicationUser
                        {
                            UserName = "user3",
                            Email = "user3@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user3, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user3, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User3 User3"),
                            new Claim(JwtClaimTypes.GivenName, "User3"),
                            new Claim(JwtClaimTypes.FamilyName, "User3"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user3 created");
                    }
                    else
                    {
                        Log.Debug("user3 already exists");
                    }

                    var user4 = userMgr.FindByNameAsync("user4").Result;
                    if (user4 == null)
                    {
                        user4 = new ApplicationUser
                        {
                            UserName = "user4",
                            Email = "user4@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user4, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user4, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User4 User4"),
                            new Claim(JwtClaimTypes.GivenName, "User4"),
                            new Claim(JwtClaimTypes.FamilyName, "User4"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user4 created");
                    }
                    else
                    {
                        Log.Debug("user4 already exists");
                    }

                    var user5 = userMgr.FindByNameAsync("user5").Result;
                    if (user5 == null)
                    {
                        user5 = new ApplicationUser
                        {
                            UserName = "user5",
                            Email = "user5@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user5, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user5, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User5 User5"),
                            new Claim(JwtClaimTypes.GivenName, "User5"),
                            new Claim(JwtClaimTypes.FamilyName, "User5"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user5 created");
                    }
                    else
                    {
                        Log.Debug("user5 already exists");
                    }

                    var user6 = userMgr.FindByNameAsync("user6").Result;
                    if (user6 == null)
                    {
                        user6 = new ApplicationUser
                        {
                            UserName = "user6",
                            Email = "user6@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user6, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user6, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User6 User6"),
                            new Claim(JwtClaimTypes.GivenName, "User6"),
                            new Claim(JwtClaimTypes.FamilyName, "User6"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user6 created");
                    }
                    else
                    {
                        Log.Debug("user6 already exists");
                    }

                    var user7 = userMgr.FindByNameAsync("user7").Result;
                    if (user7 == null)
                    {
                        user7 = new ApplicationUser
                        {
                            UserName = "user7",
                            Email = "user7@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user7, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user7, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User7 User7"),
                            new Claim(JwtClaimTypes.GivenName, "User7"),
                            new Claim(JwtClaimTypes.FamilyName, "User7"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user7 created");
                    }
                    else
                    {
                        Log.Debug("user7 already exists");
                    }

                    var user8 = userMgr.FindByNameAsync("user8").Result;
                    if (user8 == null)
                    {
                        user8= new ApplicationUser
                        {
                            UserName = "user8",
                            Email = "user8@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user8, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user8, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User8 User8"),
                            new Claim(JwtClaimTypes.GivenName, "User8"),
                            new Claim(JwtClaimTypes.FamilyName, "User8"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user8 created");
                    }
                    else
                    {
                        Log.Debug("user8 already exists");
                    }

                    var user9 = userMgr.FindByNameAsync("user9").Result;
                    if (user9 == null)
                    {
                        user9 = new ApplicationUser
                        {
                            UserName = "user9",
                            Email = "user9@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user9, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user9, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User9 User9"),
                            new Claim(JwtClaimTypes.GivenName, "User9"),
                            new Claim(JwtClaimTypes.FamilyName, "User9"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user9 created");
                    }
                    else
                    {
                        Log.Debug("user9 already exists");
                    }

                    var user10 = userMgr.FindByNameAsync("user10").Result;
                    if (user10 == null)
                    {
                        user10 = new ApplicationUser
                        {
                            UserName = "user10",
                            Email = "user10@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user10, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user10, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User10 User10"),
                            new Claim(JwtClaimTypes.GivenName, "User10"),
                            new Claim(JwtClaimTypes.FamilyName, "User10"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user10 created");
                    }
                    else
                    {
                        Log.Debug("user10 already exists");
                    }

                    //Creating roles:
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

                    if (!userMgr.IsInRoleAsync(user1, user.Name).Result)
                    {
                        Log.Debug("Adding user1 to user role");
                        _ = userMgr.AddToRoleAsync(user1, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user2, user.Name).Result)
                    {
                        Log.Debug("Adding user2 to user role");
                        _ = userMgr.AddToRoleAsync(user2, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user3, user.Name).Result)
                    {
                        Log.Debug("Adding user3 to user role");
                        _ = userMgr.AddToRoleAsync(user3, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user4, user.Name).Result)
                    {
                        Log.Debug("Adding user4 to user role");
                        _ = userMgr.AddToRoleAsync(user4, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user5, user.Name).Result)
                    {
                        Log.Debug("Adding user5 to user role");
                        _ = userMgr.AddToRoleAsync(user5, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user6, user.Name).Result)
                    {
                        Log.Debug("Adding user6 to user role");
                        _ = userMgr.AddToRoleAsync(user6, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user7, user.Name).Result)
                    {
                        Log.Debug("Adding user7 to user role");
                        _ = userMgr.AddToRoleAsync(user7, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user8, user.Name).Result)
                    {
                        Log.Debug("Adding user8 to user role");
                        _ = userMgr.AddToRoleAsync(user8, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user9, user.Name).Result)
                    {
                        Log.Debug("Adding user9 to user role");
                        _ = userMgr.AddToRoleAsync(user9, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user10, user.Name).Result)
                    {
                        Log.Debug("Adding user10 to user role");
                        _ = userMgr.AddToRoleAsync(user10, user.Name).Result;
                    }
                }
            }
        }
    }
}
