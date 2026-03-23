using Application.Services.Jwt;
using Domain.Users.Repository;
using Microsoft.AspNetCore.Http;


namespace Application.Services.CurrentUser;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IJwtServices _jwtService;


    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IJwtServices jwtService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<UserClaims?> GetValidatedUserAsync(CancellationToken ct = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            return null;

        // 🔹 Extrae claims del token
        var userClaims = _jwtService.ExtractUserClaimsFromHttpContext(httpContext);
        if (userClaims is null)
            return null;

        // 🔹 Verificamos que realmente exista en base de datos
        var user = await _userRepository.GetByIdAsync(userClaims.UserId, ct);
        if (user is null)
            return null;

        return userClaims;
    }
}
