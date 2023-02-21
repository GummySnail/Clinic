using System.Security.Claims;
using Auth.Core.Entities;
using Auth.Core.Exceptions;
using Auth.Core.Interfaces;
using Auth.Core.Services.Responses;
using Auth.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly AuthDbContext _context;

    public AuthService(
        UserManager<Account> userManager,
        SignInManager<Account> signInManager,
        IEmailService emailService,
        ITokenService tokenService,
        AuthDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task SignUpAsync(string email, string password)
    {
        var isUserExist = await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpperInvariant());

        if (isUserExist)
        {
            throw new BadRequestException("Someone already uses this email");
        }

        var account = new Account
        {
            Email = email,
            UserName = email
        };

        var createUserResult = await _userManager.CreateAsync(account, password);
        var addToRoleResult = await _userManager.AddToRoleAsync(account, "Patient");

        if (!createUserResult.Succeeded && !addToRoleResult.Succeeded)
        {
            throw new BadRequestException("Unable to create user");
        }

        var user = await _userManager.FindByEmailAsync(email.ToUpperInvariant());
        
        var token = await _emailService.GenerateEmailConfirmationTokenAsync(user);
        
        var link = _emailService.GenerateEmailConfirmationLink(token, user.Email);
        
        await _emailService.SendEmailAsync(user.Email, "Confirm your email", link);
    }

    public async Task<AuthenticatedResponse> SignInAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email.ToUpperInvariant());

        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

        if (user is null || !isPasswordCorrect)
        {
            throw new BadRequestException("Either an email or a password is incorrect");
        }

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        if (!isEmailConfirmed)
        {
            throw new BadRequestException("Email is not confirmed");
        }

        await _signInManager.PasswordSignInAsync(user, password, false, false);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
        };
        
        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }
        
        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await _context.SaveChangesAsync();
        
        return new AuthenticatedResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}