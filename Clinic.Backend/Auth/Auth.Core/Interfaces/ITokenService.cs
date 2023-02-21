using System.Security.Claims;
using Auth.Core.Services.Responses;

namespace Auth.Core.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    Task<AuthenticatedResponse> RefreshTokenAsync(string accessToken, string refreshToken);
    Task RevokeTokenAsync(string userId, string refreshToken);
}