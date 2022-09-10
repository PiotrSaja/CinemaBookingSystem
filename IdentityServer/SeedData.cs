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

                    var user11 = userMgr.FindByNameAsync("user11").Result;
                    if (user11 == null)
                    {
                        user11 = new ApplicationUser
                        {
                            UserName = "user11",
                            Email = "user11@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user11, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user11, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User11 User11"),
                            new Claim(JwtClaimTypes.GivenName, "User11"),
                            new Claim(JwtClaimTypes.FamilyName, "User11"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user11 created");
                    }
                    else
                    {
                        Log.Debug("user11 already exists");
                    }

                    var user12 = userMgr.FindByNameAsync("user12").Result;
                    if (user12 == null)
                    {
                        user12 = new ApplicationUser
                        {
                            UserName = "user12",
                            Email = "user12@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user12, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user12, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User12 User12"),
                            new Claim(JwtClaimTypes.GivenName, "User12"),
                            new Claim(JwtClaimTypes.FamilyName, "User12"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user12 created");
                    }
                    else
                    {
                        Log.Debug("user12 already exists");
                    }

                    var user13 = userMgr.FindByNameAsync("user13").Result;
                    if (user13 == null)
                    {
                        user13 = new ApplicationUser
                        {
                            UserName = "user13",
                            Email = "user13@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user13, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user13, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User13 User13"),
                            new Claim(JwtClaimTypes.GivenName, "User13"),
                            new Claim(JwtClaimTypes.FamilyName, "User13"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user13 created");
                    }
                    else
                    {
                        Log.Debug("user13 already exists");
                    }

                    var user14 = userMgr.FindByNameAsync("user14").Result;
                    if (user14 == null)
                    {
                        user14 = new ApplicationUser
                        {
                            UserName = "user14",
                            Email = "user14@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user14, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user14, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User14 User14"),
                            new Claim(JwtClaimTypes.GivenName, "User14"),
                            new Claim(JwtClaimTypes.FamilyName, "User14"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user14 created");
                    }
                    else
                    {
                        Log.Debug("user14 already exists");
                    }

                    var user15 = userMgr.FindByNameAsync("user15").Result;
                    if (user15 == null)
                    {
                        user15 = new ApplicationUser
                        {
                            UserName = "user15",
                            Email = "user15@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user15, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user15, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User15 User15"),
                            new Claim(JwtClaimTypes.GivenName, "User15"),
                            new Claim(JwtClaimTypes.FamilyName, "User15"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user15 created");
                    }
                    else
                    {
                        Log.Debug("user15 already exists");
                    }

                    var user16 = userMgr.FindByNameAsync("user16").Result;
                    if (user16 == null)
                    {
                        user16 = new ApplicationUser
                        {
                            UserName = "user16",
                            Email = "user16@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user16, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user16, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User16 User16"),
                            new Claim(JwtClaimTypes.GivenName, "User16"),
                            new Claim(JwtClaimTypes.FamilyName, "User16"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user16 created");
                    }
                    else
                    {
                        Log.Debug("user16 already exists");
                    }

                    var user17 = userMgr.FindByNameAsync("user17").Result;
                    if (user17 == null)
                    {
                        user17 = new ApplicationUser
                        {
                            UserName = "user17",
                            Email = "user17@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user17, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user17, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User17 User17"),
                            new Claim(JwtClaimTypes.GivenName, "User17"),
                            new Claim(JwtClaimTypes.FamilyName, "User17"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user17 created");
                    }
                    else
                    {
                        Log.Debug("user17 already exists");
                    }

                    var user18 = userMgr.FindByNameAsync("user18").Result;
                    if (user18 == null)
                    {
                        user18 = new ApplicationUser
                        {
                            UserName = "user18",
                            Email = "user18@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user18, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user18, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User18 User18"),
                            new Claim(JwtClaimTypes.GivenName, "User18"),
                            new Claim(JwtClaimTypes.FamilyName, "User18"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user18 created");
                    }
                    else
                    {
                        Log.Debug("user18 already exists");
                    }

                    var user19 = userMgr.FindByNameAsync("user19").Result;
                    if (user19 == null)
                    {
                        user19 = new ApplicationUser
                        {
                            UserName = "user19",
                            Email = "user19@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user19, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user19, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User19 User19"),
                            new Claim(JwtClaimTypes.GivenName, "User19"),
                            new Claim(JwtClaimTypes.FamilyName, "User19"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user19 created");
                    }
                    else
                    {
                        Log.Debug("user19 already exists");
                    }

                    var user20 = userMgr.FindByNameAsync("user20").Result;
                    if (user20 == null)
                    {
                        user20 = new ApplicationUser
                        {
                            UserName = "user20",
                            Email = "user20@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user20, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user20, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User20 User20"),
                            new Claim(JwtClaimTypes.GivenName, "User20"),
                            new Claim(JwtClaimTypes.FamilyName, "User20"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user20 created");
                    }
                    else
                    {
                        Log.Debug("user20 already exists");
                    }

                    var user21 = userMgr.FindByNameAsync("user21").Result;
                    if (user21 == null)
                    {
                        user21 = new ApplicationUser
                        {
                            UserName = "user21",
                            Email = "user21@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user21, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user21, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User21 User21"),
                            new Claim(JwtClaimTypes.GivenName, "User21"),
                            new Claim(JwtClaimTypes.FamilyName, "User21"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user21 created");
                    }
                    else
                    {
                        Log.Debug("user21 already exists");
                    }

                    var user22 = userMgr.FindByNameAsync("user22").Result;
                    if (user22 == null)
                    {
                        user22 = new ApplicationUser
                        {
                            UserName = "user22",
                            Email = "user22@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user22, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user22, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User22 User22"),
                            new Claim(JwtClaimTypes.GivenName, "User22"),
                            new Claim(JwtClaimTypes.FamilyName, "User22"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user22 created");
                    }
                    else
                    {
                        Log.Debug("user22 already exists");
                    }

                    var user23 = userMgr.FindByNameAsync("user23").Result;
                    if (user23 == null)
                    {
                        user23 = new ApplicationUser
                        {
                            UserName = "user23",
                            Email = "user23@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user23, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user23, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User23 User23"),
                            new Claim(JwtClaimTypes.GivenName, "User23"),
                            new Claim(JwtClaimTypes.FamilyName, "User23"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user23 created");
                    }
                    else
                    {
                        Log.Debug("user23 already exists");
                    }

                    var user24 = userMgr.FindByNameAsync("user24").Result;
                    if (user24 == null)
                    {
                        user24 = new ApplicationUser
                        {
                            UserName = "user24",
                            Email = "user24@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user24, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user24, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User24 User24"),
                            new Claim(JwtClaimTypes.GivenName, "User24"),
                            new Claim(JwtClaimTypes.FamilyName, "User24"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user24 created");
                    }
                    else
                    {
                        Log.Debug("user24 already exists");
                    }

                    var user25 = userMgr.FindByNameAsync("user25").Result;
                    if (user25 == null)
                    {
                        user25 = new ApplicationUser
                        {
                            UserName = "user25",
                            Email = "user25@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user25, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user25, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User25 User25"),
                            new Claim(JwtClaimTypes.GivenName, "User25"),
                            new Claim(JwtClaimTypes.FamilyName, "User25"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user25 created");
                    }
                    else
                    {
                        Log.Debug("user25 already exists");
                    }

                    var user26 = userMgr.FindByNameAsync("user26").Result;
                    if (user26 == null)
                    {
                        user26 = new ApplicationUser
                        {
                            UserName = "user26",
                            Email = "user26@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user26, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user26, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User26 User26"),
                            new Claim(JwtClaimTypes.GivenName, "User26"),
                            new Claim(JwtClaimTypes.FamilyName, "User26"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user26 created");
                    }
                    else
                    {
                        Log.Debug("user26 already exists");
                    }

                    var user27 = userMgr.FindByNameAsync("user27").Result;
                    if (user27 == null)
                    {
                        user27 = new ApplicationUser
                        {
                            UserName = "user27",
                            Email = "user27@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user27, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user27, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User27 User27"),
                            new Claim(JwtClaimTypes.GivenName, "User27"),
                            new Claim(JwtClaimTypes.FamilyName, "User27"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user27 created");
                    }
                    else
                    {
                        Log.Debug("user27 already exists");
                    }

                    var user28 = userMgr.FindByNameAsync("user28").Result;
                    if (user28 == null)
                    {
                        user28 = new ApplicationUser
                        {
                            UserName = "user28",
                            Email = "user28@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user28, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user28, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User28 User28"),
                            new Claim(JwtClaimTypes.GivenName, "User28"),
                            new Claim(JwtClaimTypes.FamilyName, "User28"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user28 created");
                    }
                    else
                    {
                        Log.Debug("user28 already exists");
                    }

                    var user29 = userMgr.FindByNameAsync("user29").Result;
                    if (user29 == null)
                    {
                        user29 = new ApplicationUser
                        {
                            UserName = "user29",
                            Email = "user29@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user29, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user29, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User29 User29"),
                            new Claim(JwtClaimTypes.GivenName, "User29"),
                            new Claim(JwtClaimTypes.FamilyName, "User29"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user29 created");
                    }
                    else
                    {
                        Log.Debug("user29 already exists");
                    }

                    var user30 = userMgr.FindByNameAsync("user30").Result;
                    if (user30 == null)
                    {
                        user30 = new ApplicationUser
                        {
                            UserName = "user30",
                            Email = "user30@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user30, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user30, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User30 User30"),
                            new Claim(JwtClaimTypes.GivenName, "User30"),
                            new Claim(JwtClaimTypes.FamilyName, "User30"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user30 created");
                    }
                    else
                    {
                        Log.Debug("user30 already exists");
                    }

                    var user31 = userMgr.FindByNameAsync("user31").Result;
                    if (user31 == null)
                    {
                        user31 = new ApplicationUser
                        {
                            UserName = "user31",
                            Email = "user31@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user31, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user31, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User31 User31"),
                            new Claim(JwtClaimTypes.GivenName, "User31"),
                            new Claim(JwtClaimTypes.FamilyName, "User31"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user31 created");
                    }
                    else
                    {
                        Log.Debug("user31 already exists");
                    }

                    var user32 = userMgr.FindByNameAsync("user32").Result;
                    if (user32 == null)
                    {
                        user32 = new ApplicationUser
                        {
                            UserName = "user32",
                            Email = "user32@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user32, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user32, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User32 User32"),
                            new Claim(JwtClaimTypes.GivenName, "User32"),
                            new Claim(JwtClaimTypes.FamilyName, "User32"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user32 created");
                    }
                    else
                    {
                        Log.Debug("user32 already exists");
                    }

                    var user33 = userMgr.FindByNameAsync("user33").Result;
                    if (user33 == null)
                    {
                        user33 = new ApplicationUser
                        {
                            UserName = "user33",
                            Email = "user33@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user33, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user33, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User33 User33"),
                            new Claim(JwtClaimTypes.GivenName, "User33"),
                            new Claim(JwtClaimTypes.FamilyName, "User33"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user33 created");
                    }
                    else
                    {
                        Log.Debug("user33 already exists");
                    }

                    var user34 = userMgr.FindByNameAsync("user34").Result;
                    if (user34 == null)
                    {
                        user34 = new ApplicationUser
                        {
                            UserName = "user34",
                            Email = "user34@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user34, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user34, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User34 User34"),
                            new Claim(JwtClaimTypes.GivenName, "User34"),
                            new Claim(JwtClaimTypes.FamilyName, "User34"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user34 created");
                    }
                    else
                    {
                        Log.Debug("user34 already exists");
                    }

                    var user35 = userMgr.FindByNameAsync("user35").Result;
                    if (user35 == null)
                    {
                        user35 = new ApplicationUser
                        {
                            UserName = "user35",
                            Email = "user35@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user35, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user35, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User35 User35"),
                            new Claim(JwtClaimTypes.GivenName, "User35"),
                            new Claim(JwtClaimTypes.FamilyName, "User35"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user35 created");
                    }
                    else
                    {
                        Log.Debug("user35 already exists");
                    }

                    var user36 = userMgr.FindByNameAsync("user36").Result;
                    if (user36 == null)
                    {
                        user36 = new ApplicationUser
                        {
                            UserName = "user36",
                            Email = "user36@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user36, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user36, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User36 User36"),
                            new Claim(JwtClaimTypes.GivenName, "User36"),
                            new Claim(JwtClaimTypes.FamilyName, "User36"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user36 created");
                    }
                    else
                    {
                        Log.Debug("user36 already exists");
                    }

                    var user37 = userMgr.FindByNameAsync("user37").Result;
                    if (user37 == null)
                    {
                        user37 = new ApplicationUser
                        {
                            UserName = "user37",
                            Email = "user37@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user37, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user37, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User37 User37"),
                            new Claim(JwtClaimTypes.GivenName, "User37"),
                            new Claim(JwtClaimTypes.FamilyName, "User37"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user37 created");
                    }
                    else
                    {
                        Log.Debug("user37 already exists");
                    }

                    var user38 = userMgr.FindByNameAsync("user38").Result;
                    if (user38 == null)
                    {
                        user38 = new ApplicationUser
                        {
                            UserName = "user38",
                            Email = "user38@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user38, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user38, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User38 User38"),
                            new Claim(JwtClaimTypes.GivenName, "User38"),
                            new Claim(JwtClaimTypes.FamilyName, "User38"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user38 created");
                    }
                    else
                    {
                        Log.Debug("user38 already exists");
                    }

                    var user39 = userMgr.FindByNameAsync("user39").Result;
                    if (user39 == null)
                    {
                        user39 = new ApplicationUser
                        {
                            UserName = "user39",
                            Email = "user39@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user39, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user39, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User39 User39"),
                            new Claim(JwtClaimTypes.GivenName, "User39"),
                            new Claim(JwtClaimTypes.FamilyName, "User39"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user39 created");
                    }
                    else
                    {
                        Log.Debug("user39 already exists");
                    }

                    var user40 = userMgr.FindByNameAsync("user39").Result;
                    if (user40 == null)
                    {
                        user40 = new ApplicationUser
                        {
                            UserName = "user40",
                            Email = "user40@example.com",
                            EmailConfirmed = true,
                        };
                        var result = userMgr.CreateAsync(user40, "Pass123$").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(user40, new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "User40 User40"),
                            new Claim(JwtClaimTypes.GivenName, "User40"),
                            new Claim(JwtClaimTypes.FamilyName, "User40"),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Debug("user40 created");
                    }
                    else
                    {
                        Log.Debug("user40 already exists");
                    }

                    #endregion

                    #region Roles
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

                    if (!userMgr.IsInRoleAsync(user11, user.Name).Result)
                    {
                        Log.Debug("Adding user11 to user role");
                        _ = userMgr.AddToRoleAsync(user11, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user12, user.Name).Result)
                    {
                        Log.Debug("Adding user12 to user role");
                        _ = userMgr.AddToRoleAsync(user12, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user13, user.Name).Result)
                    {
                        Log.Debug("Adding user13 to user role");
                        _ = userMgr.AddToRoleAsync(user13, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user14, user.Name).Result)
                    {
                        Log.Debug("Adding user14 to user role");
                        _ = userMgr.AddToRoleAsync(user14, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user15, user.Name).Result)
                    {
                        Log.Debug("Adding user15 to user role");
                        _ = userMgr.AddToRoleAsync(user15, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user16, user.Name).Result)
                    {
                        Log.Debug("Adding user16 to user role");
                        _ = userMgr.AddToRoleAsync(user16, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user17, user.Name).Result)
                    {
                        Log.Debug("Adding user17 to user role");
                        _ = userMgr.AddToRoleAsync(user17, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user18, user.Name).Result)
                    {
                        Log.Debug("Adding user18 to user role");
                        _ = userMgr.AddToRoleAsync(user18, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user19, user.Name).Result)
                    {
                        Log.Debug("Adding user19 to user role");
                        _ = userMgr.AddToRoleAsync(user19, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user20, user.Name).Result)
                    {
                        Log.Debug("Adding user20 to user role");
                        _ = userMgr.AddToRoleAsync(user20, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user21, user.Name).Result)
                    {
                        Log.Debug("Adding user21 to user role");
                        _ = userMgr.AddToRoleAsync(user21, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user22, user.Name).Result)
                    {
                        Log.Debug("Adding user22 to user role");
                        _ = userMgr.AddToRoleAsync(user22, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user23, user.Name).Result)
                    {
                        Log.Debug("Adding user23 to user role");
                        _ = userMgr.AddToRoleAsync(user23, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user24, user.Name).Result)
                    {
                        Log.Debug("Adding user24 to user role");
                        _ = userMgr.AddToRoleAsync(user24, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user25, user.Name).Result)
                    {
                        Log.Debug("Adding user25 to user role");
                        _ = userMgr.AddToRoleAsync(user25, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user26, user.Name).Result)
                    {
                        Log.Debug("Adding user26 to user role");
                        _ = userMgr.AddToRoleAsync(user26, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user27, user.Name).Result)
                    {
                        Log.Debug("Adding user27 to user role");
                        _ = userMgr.AddToRoleAsync(user27, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user28, user.Name).Result)
                    {
                        Log.Debug("Adding user28 to user role");
                        _ = userMgr.AddToRoleAsync(user28, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user29, user.Name).Result)
                    {
                        Log.Debug("Adding user29 to user role");
                        _ = userMgr.AddToRoleAsync(user29, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user30, user.Name).Result)
                    {
                        Log.Debug("Adding user30 to user role");
                        _ = userMgr.AddToRoleAsync(user30, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user31, user.Name).Result)
                    {
                        Log.Debug("Adding user31 to user role");
                        _ = userMgr.AddToRoleAsync(user31, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user32, user.Name).Result)
                    {
                        Log.Debug("Adding user32 to user role");
                        _ = userMgr.AddToRoleAsync(user32, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user33, user.Name).Result)
                    {
                        Log.Debug("Adding user33 to user role");
                        _ = userMgr.AddToRoleAsync(user33, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user34, user.Name).Result)
                    {
                        Log.Debug("Adding user34 to user role");
                        _ = userMgr.AddToRoleAsync(user34, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user35, user.Name).Result)
                    {
                        Log.Debug("Adding user35 to user role");
                        _ = userMgr.AddToRoleAsync(user35, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user36, user.Name).Result)
                    {
                        Log.Debug("Adding user36 to user role");
                        _ = userMgr.AddToRoleAsync(user36, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user37, user.Name).Result)
                    {
                        Log.Debug("Adding user37 to user role");
                        _ = userMgr.AddToRoleAsync(user37, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user38, user.Name).Result)
                    {
                        Log.Debug("Adding user38 to user role");
                        _ = userMgr.AddToRoleAsync(user38, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user39, user.Name).Result)
                    {
                        Log.Debug("Adding user39 to user role");
                        _ = userMgr.AddToRoleAsync(user39, user.Name).Result;
                    }

                    if (!userMgr.IsInRoleAsync(user40, user.Name).Result)
                    {
                        Log.Debug("Adding user40 to user role");
                        _ = userMgr.AddToRoleAsync(user40, user.Name).Result;
                    }
                    #endregion
                }
            }
        }
    }
}
