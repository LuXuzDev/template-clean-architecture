using Application.Features.Auth.GenerateRefreshToken;
using Application.Features.Auth.Shared.Response;
using Application.Services.Jwt;
using Domain.Entities.RefreshTokens.Repository;
using Domain.Entities.Users.Repository;
using Domain.Specifications.Users;
using FastEndpoints;
using Shared.Results;
using Shared.Results.Errors.Auth;
using Shared.Results.Errors.RefreshToken;
using Shared.Results.Errors.User;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenCommandHandler : CommandHandler<RefreshTokenCommand, Result<AuthTokenResponse>>
{
    private readonly IJwtServices _jwtServices;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public RefreshTokenCommandHandler
        (IJwtServices jwtServices,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _jwtServices = jwtServices;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
    }
    public async override Task<Result<AuthTokenResponse>> ExecuteAsync(RefreshTokenCommand command, CancellationToken ct = default)
    {
        var req = command.Request;

        using var sha256 = SHA256.Create();
        var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(req.RefreshToken));
        var refreshTokenCompare = Convert.ToBase64String(refreshTokenHash);


        var existRefreshToken = await _refreshTokenRepository.GetByRefreshTokenAsync(refreshTokenCompare, ct);

        if (existRefreshToken is null)
            return Result<AuthTokenResponse>.Failure(RefreshTokenErrors.NotFound);

        if (existRefreshToken!.CreatedAt.AddHours(JwtSettings.RefreshTokenExpirationInMinutes) < DateTime.UtcNow)
            return Result<AuthTokenResponse>.Failure(AuthErrors.TokenExpired);


        var userEntity = await _userRepository.FirstOrDefaultAsync(new UserByIdSpecification(existRefreshToken.UserId), ct);
        if (userEntity is null)
            return Result<AuthTokenResponse>.Failure(UserErrors.NotFound);

        var commandRf = new GenerateRefreshTokenCommand
        {
            Request = new GenerateRefreshTokenRequest
            {
                UserId = userEntity!.Id.ToString(),
            }
        };

        var refreshToken = await commandRf.ExecuteAsync(ct);

        var replaceHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken.Value!));
        existRefreshToken!.ReplacedByTokenHash = Convert.ToBase64String(replaceHash);
        existRefreshToken!.RevokedAt = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(existRefreshToken);

        return Result<AuthTokenResponse>.Success(new AuthTokenResponse
        {
            Token = _jwtServices.GenerateToken(userEntity!),
            RefreshToken = refreshToken.Value!,
            ExpiresToken = JwtSettings.ExpirationInMinutes,
            ExpiresRefreshToken = JwtSettings.RefreshTokenExpirationInMinutes,
            Role = userEntity.Role!.Name,
        });
    }
}
