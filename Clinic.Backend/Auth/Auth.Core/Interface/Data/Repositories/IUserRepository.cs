using Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Interface.Data.Repositories;

public interface IUserRepository
{
    Task<bool> IsUserExistByEmailAsync(string email);
    Task<IdentityResult> CreateUserAsync(string email, string password);
    Task<Account> GetUserByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(Account user, string password);
    Task<SignInResult> PasswordSignInAsync(Account user, string password);
    Task<bool> CheckEmailConfirmation(Account user);
    Task LogoutAsync();
}