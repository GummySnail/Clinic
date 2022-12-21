﻿using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4;
using IdentityServer4.Test;

namespace Auth.Api.Configuration;

public static class IdentityServerConfiguration
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new("user-profile", "Your profile data", 
                new List<string>{ "name", "role", "email" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("UserInfoScope",new List<string>
            {
                JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.ClientId
            })
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("Client")
            {
                Scopes =
                {
                    "UserInfoScope"
                }
            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientName = "ClinicClient",
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    "UserInfoScope",
                    "user-profile",
                    "Client"
                },
                ClientSecrets =
                {
                    new Secret("client-secret".Sha256())
                },
                AccessTokenLifetime = 180,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true
            }
        };
}