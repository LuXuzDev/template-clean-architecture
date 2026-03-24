namespace Application.Features.Auth.Logout;

public class LogoutRequest
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
}
