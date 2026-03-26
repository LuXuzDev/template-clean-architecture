using Domain.Entities.Users.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Jwt;

public interface IJwtServices
{

    string GenerateToken(User user);

    UserClaims? ExtractUserClaimsFromHttpContext(HttpContext context);

    UserClaims? GetUserClaimsFromToken(string token);

}