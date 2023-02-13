using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4;

namespace Auth.Api.Configuration;

public static class IdentityServerConfiguration
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new("user-profile", "Your profile data", 
                new List<string>{ "name", "role", "email" }),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("UserInfoScope",new List<string>
            {
                JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.ClientId
            }),
            new("ApiScope")
        };

    public static IEnumerable<ApiResource> ApiResources(IConfiguration config) =>
        new List<ApiResource>
        {
            new("Client")
            {
                ApiSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                Scopes =
                {
                    "UserInfoScope"
                }
            },
            new("Profiles.Api")
            {
                ApiSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                Scopes =
                {
                    "UserInfoScope",
                    "ApiScope"
                }
            },
            new("Offices.Api")
            {
                ApiSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                Scopes =
                {
                    "UserInfoScope",
                    "ApiScope"
                }
            },
            new("Services.Api")
            {
                ApiSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                Scopes =
                {
                    "UserInfoScope",
                    "ApiScope"
                }
            },
            new("Appointments.Api")
            {
                ApiSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                Scopes =
                {
                    "UserInfoScope",
                    "ApiScope"
                }
            },
            new("Documents.Api")
            {
                ApiSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                Scopes =
                {
                    "UserInfoScope",
                    "ApiScope"
                }
            }
        };

    public static IEnumerable<Client> Clients(IConfiguration config) =>
        new List<Client>
        {
            new()
            {
                ClientId = "client",
                ClientSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = new List<string>{ "https://localhost:5005/signin-oidc" },
                PostLogoutRedirectUris = {"https://localhost:5005/signout-callback-oidc"},
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    "UserInfoScope",
                    "user-profile",
                    "Client",
                    IdentityServerConstants.StandardScopes.OfflineAccess
                },
                RequireConsent = false,
                AccessTokenLifetime = 180,
                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RequirePkce = true,
            },
            new()
            {
                ClientId = "api",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret(config["ClientSecret"].Sha256())
                },
                AllowedScopes =
                {
                  "ApiScope"  
                },
                AllowOfflineAccess = true
            }
        };
}