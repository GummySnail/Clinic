using Auth.Core.Interface.Data.Repositories;
using Auth.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpperInvariant());
    }

    public async Task<IdentityResult> CreateUserAsync(string email, string password)
    {
        var user = new User
        {
            Email = email,
            UserName = email
        };

        var createUserResult = await _userManager.CreateAsync(user, password);
        var addToRoleResult = await _userManager.AddToRoleAsync(user, "Patient");
        
        if (createUserResult.Succeeded && addToRoleResult.Succeeded)
        {
            return IdentityResult.Success;
        }

        return IdentityResult.Failed();
    }
}