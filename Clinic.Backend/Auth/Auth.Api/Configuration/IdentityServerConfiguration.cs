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
                new List<string>{ "name", "role", "email" })
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("UserInfoScope",new List<string>
            {
                JwtClaimTypes.Subject, JwtClaimTypes.Name, JwtClaimTypes.Role, JwtClaimTypes.ClientId
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
                ClientId = "client",
                ClientSecrets =
                {
                    new Secret("client-secret".Sha256())
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
            }
        };
}