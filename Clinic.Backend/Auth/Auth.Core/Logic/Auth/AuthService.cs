using Auth.Core.Interface.Data.Repositories;
using Auth.Core.Interface.Services;

namespace Auth.Core.Logic.Auth;

public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public AuthService(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
    }

    public async Task<string> SignUpAsync(string email, string password)
    {
        if (await _userRepository.IsUserExistByEmailAsync(email))
        {
            return "Someone already uses this email";
        }

        var result = await _userRepository.CreateUserAsync(email, password);

        if (!result.Succeeded)
        {
            return "Unable to create user";
        }
        
        var user = await _userRepository.GetUserByEmailAsync(email);
        
        var token = await _emailService.GenerateEmailConfirmationTokenAsync(user);

        var link = _emailService.GenerateEmailConfirmationLink(token, user.Email);

        await _emailService.SendEmailAsync(user.Email, "Confirm your email", link);
        
        return "";
    }

    public async Task<string> SignInAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user is null || !await _userRepository.CheckPasswordAsync(user, password))
        {
            return "Either an email or a password is incorrect";
        }

        if (!await _userRepository.CheckEmailConfirmation(user))
        {
            return "Email is not confirmed";
        }
        
        var result = await _userRepository.PasswordSignInAsync(user, password);

        return "";
    }
    
    public async Task ConfirmEmailAsync(string email, string token)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        
        if (user is null)
        {
            //throw new NotFoundException("User is not exist");
        }
        var result = await _emailService.ConfirmEmailAsync(user, token);

        if (result == false)
        {
            //throw new EmailConfirmationException("Unable to confirm email");
        }
    }

    public async Task logoutAsync()
    {
        _userRepository.LogoutAsync();
    }
}