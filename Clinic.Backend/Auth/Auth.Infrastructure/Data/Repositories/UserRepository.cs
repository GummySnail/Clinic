using Auth.Core.Entities;
using Auth.Core.Interface.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Auth.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;

    public UserRepository(UserManager<Account> userManager, SignInManager<Account> signInManager)
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
        var user = new Account
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

    public async Task<Account> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email.ToUpperInvariant());
    }

    public async Task<bool> CheckPasswordAsync(Account user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<SignInResult> PasswordSignInAsync(Account user, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
        
        return result;
    }

    public async Task<bool> CheckEmailConfirmation(Account user)
    {
        return await _userManager.IsEmailConfirmedAsync(user);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}