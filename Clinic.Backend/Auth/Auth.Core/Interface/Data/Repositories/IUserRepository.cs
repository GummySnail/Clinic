using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Interface.Data.Repositories;

public interface IUserRepository
{
    Task<bool> IsUserExistByEmailAsync(string email);
    Task<IdentityResult> CreateUserAsync(string email, string password);
    /*Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByIdAsync(string id);*/
    /*Task<bool> CheckPasswordAsync(IdentityUser user, string password);
    Task<bool> CheckEmailConfirmation(IdentityUser user);
    Task<IList<string>> GetUserRoleAsync(IdentityUser user);*/
}