using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Interface.Services;

public interface IEmailService
{
    Task<string> GenerateEmailConfirmationTokenAsync(IdentityUser user);
    string GenerateEmailConfirmationLink(string confirmEmailToken, string email);
    Task SendEmailAsync(string recipientEmail, string subject, string urlString);
    Task<bool> ConfirmEmailAsync(IdentityUser user, string token);
}