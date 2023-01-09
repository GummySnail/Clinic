namespace Auth.Core.Interface.Services;

public interface IAuthService
{
    public  Task<string> SignUpAsync(string email, string password);
    public Task<string> SignInAsync(string email, string password);
    public  Task ConfirmEmailAsync(string email, string token);
    public  Task LogoutAsync();
}