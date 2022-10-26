// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(name: "user", userClaims: new[]
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.Role,
                    JwtClaimTypes.Id
                })
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("api")
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "Client for Swagger user",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedScopes = {"api","user","openid"},
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"https://saja.website:44351/swagger/oauth2-redirect.html"},
                    AllowedCorsOrigins = { "https://saja.website:44351" }
                },

                new Client
                {
                    ClientId = "vue",
                    ClientName = "Client for vue user",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes = {"api","user","openid"},
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"https://saja.website/login"},
                    PostLogoutRedirectUris = { "https://saja.website/logout" },
                    AllowedCorsOrigins = { "https://saja.website" }
                },

                new Client
                {
                    ClientId = "vue-admin",
                    ClientName = "Client for vue admin user",
                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedScopes = {"api","user","openid"},
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = {"https://saja.website:44301/login"},
                    PostLogoutRedirectUris = { "https://saja.website:44301/logout" },
                    AllowedCorsOrigins = { "https://saja.website:44301" }
                }
            };
    }
}