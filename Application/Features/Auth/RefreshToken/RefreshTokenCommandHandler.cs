using Application.Features.Auth.GenerateRefreshToken;
using Application.Features.Auth.Shared.Response;
using Application.Helpers.Hasher;
using Application.Services.Jwt;
using Domain.Entities.RefreshTokens.Repository;
using Domain.Entities.Users.Repository;
using Domain.Shared.Abstractions;
using Domain.Specifications.RefreshTokens;
using Domain.Specifications.Users;
using FastEndpoints;
using Shared.Results;
using Shared.Results.Errors.Auth;
using Shared.Results.Errors.RefreshToken;
using Shared.Results.Errors.User;

namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenCommandHandler : CommandHandler<RefreshTokenCommand, Result<AuthTokenResponse>>
{
    private readonly IJwtServices _jwtServices;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenCommandHandler
        (IJwtServices jwtServices,
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _jwtServices = jwtServices;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }
    public async override Task<Result<AuthTokenResponse>> ExecuteAsync(RefreshTokenCommand command, CancellationToken ct = default)
    {
        var req = command.Request;

        var refreshTokenHash = HasherHelper.ComputeRefreshTokenHash(req.RefreshToken);

        var existRefreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(new RefreshTokenByTokenSpecification(refreshTokenHash), ct);

        if (existRefreshToken is null)
            return Result<AuthTokenResponse>.Failure(RefreshTokenError.NotFound);

        if (existRefreshToken!.CreatedAt.AddHours(JwtSettings.RefreshTokenExpirationInMinutes) < DateTime.UtcNow)
            return Result<AuthTokenResponse>.Failure(AuthError.TokenExpired);


        var userEntity = await _userRepository.FirstOrDefaultAsync(new UserByIdSpecification(existRefreshToken.UserId), ct);
        if (userEntity is null)
            return Result<AuthTokenResponse>.Failure(UserError.NotFound);

        var commandRf = new GenerateRefreshTokenCommand
        {
            Request = new GenerateRefreshTokenRequest
            {
                UserId = userEntity!.Id.ToString(),
            }
        };

        var refreshToken = await commandRf.ExecuteAsync(ct);

        existRefreshToken!.ReplacedByTokenHash = HasherHelper.ComputeRefreshTokenHash(refreshToken.Value!);
        existRefreshToken!.RevokedAt = DateTime.UtcNow;


        await _refreshTokenRepository.UpdateAsync(existRefreshToken , ct);

        await _unitOfWork.SaveChangesAsync(ct);

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
