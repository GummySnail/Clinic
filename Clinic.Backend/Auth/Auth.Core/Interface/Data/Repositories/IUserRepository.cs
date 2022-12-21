using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Interface.Data.Repositories;

public interface IUserRepository
{
    Task<bool> IsUserExistByEmailAsync(string email);
    Task<IdentityResult> CreateUserAsync(string email, string password);
    Task<IdentityUser> GetUserByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(IdentityUser user, string password);
    Task<SignInResult> PasswordSignInAsync(IdentityUser user, string password);
    Task<bool> CheckEmailConfirmation(IdentityUser user);
    Task LogoutAsync();
}