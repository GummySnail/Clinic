using Auth.Core.Interface.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UserRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> IsUserExistByEmailAsync(string email)
    {
        return await _userManager.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpperInvariant());
    }

    public async Task<IdentityResult> CreateUserAsync(string email, string password)
    {
        var user = new IdentityUser
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

    public async Task<IdentityUser> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email.ToUpperInvariant());
    }

    public async Task<bool> CheckPasswordAsync(IdentityUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<SignInResult> PasswordSignInAsync(IdentityUser user, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        
        return result;
    }
}