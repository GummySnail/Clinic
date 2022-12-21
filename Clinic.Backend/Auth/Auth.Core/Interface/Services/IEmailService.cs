using Auth.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Interface.Services;

public interface IEmailService
{
    Task<string> GenerateEmailConfirmationTokenAsync(Account user);
    string GenerateEmailConfirmationLink(string confirmEmailToken, string email);
    Task SendEmailAsync(string recipientEmail, string subject, string urlString);
    Task<bool> ConfirmEmailAsync(Account user, string token);
}