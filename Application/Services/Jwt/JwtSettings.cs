

namespace Application.Services.Jwt;

public class JwtSettings
{
    public static string SecretKey { get; set; }
    public static string Issuer { get; set; }
    public static string Audience { get; set; }
    public static int ExpirationInMinutes { get; set; } = 60;
    public static int RefreshTokenExpirationInMinutes { get; set; } = 43200;
}
