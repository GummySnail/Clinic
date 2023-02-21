namespace Auth.Core.Services.Responses;

public class AuthenticatedResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}