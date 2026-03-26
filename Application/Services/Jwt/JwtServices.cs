using Domain.Entities.Users.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Application.Services.Jwt;


public sealed class JwtServices : IJwtServices
{
    private readonly byte[] _secretKey;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="JwtService"/>.
    /// </summary>
    public JwtServices()
    {
        _secretKey = Encoding.UTF8.GetBytes(JwtSettings.SecretKey);
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        // Solo el rol importa
        if (user.Role is not null)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(JwtSettings.ExpirationInMinutes),
            Issuer = JwtSettings.Issuer,
            Audience = JwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_secretKey),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    private ClaimsPrincipal? ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = JwtSettings.Issuer,
                ValidAudience = JwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(_secretKey)
            };
            
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            return null;
        }
    }

    public UserClaims? GetUserClaimsFromToken(string token)
    {
        var principal = ValidateToken(token);
        if (principal is null)
            return null;
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return new UserClaims
        {
            UserId = new Guid(userIdClaim!),
            Role = principal.FindFirst(ClaimTypes.Role)?.Value
        };
    }


    public UserClaims? ExtractUserClaimsFromHttpContext(HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            return null;

        var token = authHeader["Bearer ".Length..].Trim();
        return GetUserClaimsFromToken(token);
    }
}