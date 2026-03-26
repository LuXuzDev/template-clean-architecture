using Application.Services.Jwt;
using Domain.Entities.RefreshTokens.Repository;
using Domain.Entities.Users.Repository;
using Domain.Specifications.Users;
using Microsoft.AspNetCore.Http;


namespace Application.Services.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IJwtServices _jwtService;
    private readonly ITokenBlackListRepository _tokenBlackListRepository;


    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IJwtServices jwtService,
        ITokenBlackListRepository tokenBlackListRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _jwtService = jwtService;
        _tokenBlackListRepository = tokenBlackListRepository;
    }

    public async Task<UserClaims?> GetValidatedUserAsync(CancellationToken ct = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            return null;

        var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null; // o manejar 401

        // Extraer solo el token
        var accessToken = authHeader.Substring("Bearer ".Length).Trim();

        if (await _tokenBlackListRepository.ExistsAsync(accessToken, ct))
            return null; // token bloqueado

        if (await _tokenBlackListRepository.ExistsAsync(accessToken, ct))
            return null;

        // 🔹 Extrae claims del token
        var userClaims = _jwtService.ExtractUserClaimsFromHttpContext(httpContext);
        if (userClaims is null)
            return null;
        
        // 🔹 Verificamos que realmente exista en base de datos
        var user = await _userRepository.FirstOrDefaultAsync(new UserByIdSpecification(userClaims.UserId), ct);
        if (user is null)
            return null;

        return userClaims;
    }
}
