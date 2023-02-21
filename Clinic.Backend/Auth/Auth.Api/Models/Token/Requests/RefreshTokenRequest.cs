namespace Auth.Api.Models.Token.Requests;

public record RefreshTokenRequest(string AccessToken, string RefreshToken);