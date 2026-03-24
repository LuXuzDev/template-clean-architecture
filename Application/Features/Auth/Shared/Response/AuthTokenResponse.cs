namespace Application.Features.Auth.Shared.Response;

public class AuthTokenResponse
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public required long ExpiresToken { get; set; }
    public required long ExpiresRefreshToken { get; set; }
    public required string Role { get; set; }
}