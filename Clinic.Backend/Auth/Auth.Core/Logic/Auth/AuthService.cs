using Auth.Core.Interface.Data.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Logic.Auth;

public class AuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string?> SignUpAsync(string email, string password)
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
        
        return "";
    }

    public async Task<string> SignInAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);

        if (user is null && !await _userRepository.CheckPasswordAsync(user, password))
        {
            return "Either an email or a password is incorrect";
        }

        var result = await _userRepository.PasswordSignInAsync(user, password);

        return "";
    }
}