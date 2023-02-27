using MailKit.Net.Smtp;
using System.Web;
using Auth.Core.Entities;
using Auth.Core.Interfaces;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Auth.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly UserManager<Account> _userManager;
    private readonly IConfiguration _config;

    public EmailService(UserManager<Account> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(Account user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        return token;
    }

    public string GenerateEmailConfirmationLink(string confirmEmailToken, string email)
    {
        var uriBuilder = new UriBuilder(_config["ReturnPaths:ConfirmEmail"]);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["email"] = email;
        query["token"] = confirmEmailToken;
        uriBuilder.Query = query.ToString();
        var urlString = uriBuilder.ToString();
        
        return urlString;
    }

    public async Task SendEmailAsync(string recipientEmail, string subject, string urlString)
    {
        var bodyBuilder = new BodyBuilder();

        bodyBuilder.HtmlBody = $"<a href=\"{urlString}\">Verify Email</a>";
        
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress(_config["SMTP:SenderName"], _config["SMTP:SenderEmail"]));
        message.To.Add(MailboxAddress.Parse(recipientEmail));
        message.Subject = subject;
        message.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(_config["SMTP:Server"], int.Parse(_config["SMTP:Port"]), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config["SMTP:SenderEmail"], _config["SMTP:Password"]);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }

    public async Task<bool> ConfirmEmailAsync(Account user, string token)
    {
        var result = await _userManager.ConfirmEmailAsync(user, token);

        return result.Succeeded;
    }
}