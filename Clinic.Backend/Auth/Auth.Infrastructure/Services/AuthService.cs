using Auth.Core.Entities;
using Auth.Core.Interface.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IEmailService _emailService;
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;

    public AuthService(
        IEmailService emailService,
        SignInManager<Account> signInManager,
        UserManager<Account> userManager)
    {
        _emailService = emailService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<string> SignUpAsync(string email, string password)
    {
        var isUserExist = await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpperInvariant());
        
        if (isUserExist)
        {
            return "Someone already uses this email";
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
            return "Unable to create user";
        }

        var user = await _userManager.FindByEmailAsync(email.ToUpperInvariant());
        
        var token = await _emailService.GenerateEmailConfirmationTokenAsync(user);

        var link = _emailService.GenerateEmailConfirmationLink(token, user.Email);

        await _emailService.SendEmailAsync(user.Email, "Confirm your email", link);
        
        return "";
    }

    public async Task<string> SignInAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email.ToUpperInvariant());
        
        var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

        if (user is null || !isPasswordCorrect)
        {
            return "Either an email or a password is incorrect";
        }

        var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
        {
            return "Email is not confirmed";
        }
        
        await _signInManager.PasswordSignInAsync(user, password, false, false);

        return "";
    }
    
    public async Task ConfirmEmailAsync(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email.ToUpperInvariant());
        await _emailService.ConfirmEmailAsync(user, token);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
        
    }
}