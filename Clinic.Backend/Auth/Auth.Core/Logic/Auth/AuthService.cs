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

    public async Task<IdentityResult> SignUpAsync(string email, string password)
    {
        if (await _userRepository.IsUserExistByEmailAsync(email))
        {
            return IdentityResult.Failed();
        }

        var result = await _userRepository.CreateUserAsync(email, password);

        return result;
    }
}