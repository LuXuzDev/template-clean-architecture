using Application.Services.CurrentUser;
using Application.Services.Jwt;
using Shared.Results;
using Shared.Results.Errors.Auth;


namespace Application.Services.UserValidator;


public class UserValidatorService : IUserValidatorService
{
    private readonly ICurrentUserService _currentUserService;

    public UserValidatorService(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<Result<UserClaims>> ValidateAsync(
        CancellationToken ct,
        params string[] roles)
    {
        var userClaims = await _currentUserService.GetValidatedUserAsync(ct);

        if (userClaims is null)
            return Result<UserClaims>.Failure(AuthError.Unauthorized);

        if (roles.Any() && !roles.Any(r => userClaims!.Role!.Contains(r)))
            return Result<UserClaims>.Failure(AuthError.Forbidden);

        return Result<UserClaims>.Success(userClaims);
    }
}
