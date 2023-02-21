using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Auth.Core.Exceptions;
using Auth.Core.Interfaces;
using Auth.Core.Services.Responses;
using Auth.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    private readonly AuthDbContext _context;

    public TokenService(IConfiguration config, AuthDbContext context)
    {
        _config = config;
        _context = context;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SecretKey"]));
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        
        randomNumberGenerator.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _config["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = _config["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _key,
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal =
            tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid access token");
        }

        return principal;
    }

    public async Task<AuthenticatedResponse> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        var principal = GetPrincipalFromExpiredToken(accessToken);
        var email = principal.FindFirst(ClaimTypes.Email)!.Value;

        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new BadRequestException("Invalid client request");
        }
        
        var newAccessToken = GenerateAccessToken(principal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _context.SaveChangesAsync();

        return new AuthenticatedResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public async Task RevokeTokenAsync(string userId, string refreshToken)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            throw new BadRequestException("Unable to logout");
        }

        user.RefreshToken = null;
        await _context.SaveChangesAsync();
    }
}