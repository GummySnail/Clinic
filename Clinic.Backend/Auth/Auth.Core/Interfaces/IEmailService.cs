using Auth.Core.Entities;

namespace Auth.Core.Interfaces;

public interface IEmailService
{
    Task<string> GenerateEmailConfirmationTokenAsync(Account user);
    string GenerateEmailConfirmationLink(string confirmEmailToken, string email);
    Task SendEmailAsync(string recipientEmail, string subject, string urlString);
    Task<bool> ConfirmEmailAsync(Account user, string token);
}