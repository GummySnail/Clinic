using Auth.Core.Services.Responses;

namespace Auth.Core.Interfaces;

public interface IAuthService
{
    Task SignUpAsync(string email, string password);
    Task<AuthenticatedResponse> SignInAsync(string email, string password);
    Task ConfirmEmailAsync(string email, string token);
}