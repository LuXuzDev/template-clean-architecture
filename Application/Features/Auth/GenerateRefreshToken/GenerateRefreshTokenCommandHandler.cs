using Application.Helpers.Hasher;
using Application.Services.Jwt;
using Domain.Entities.RefreshTokens.Repository;
using Domain.Shared.Abstractions;
using FastEndpoints;
using Shared.Results;
using System.Security.Cryptography;


namespace Application.Features.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenCommandHandler : CommandHandler<GenerateRefreshTokenCommand, Result<string>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateRefreshTokenCommandHandler
        (IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async override Task<Result<string>> ExecuteAsync(GenerateRefreshTokenCommand command, CancellationToken ct = default)
    {
        var req = command.Request;

        var refreshTokenString = GenerateRandomString(JwtSettings.SecretKey);
        var refreshTokenHash = HasherHelper.ComputeRefreshTokenHash(refreshTokenString);

        DateTime created = DateTime.UtcNow;
        DateTime expired = DateTime.UtcNow.AddMinutes(JwtSettings.RefreshTokenExpirationInMinutes);

        var refreshTokenEntity = new Domain.Entities.RefreshTokens.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(req.UserId),
            TokenHash = refreshTokenHash,
            CreatedAt = created,
            ExpiresAt = expired,

        };
        await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result<string>.Success(refreshTokenString);
    }

    private string GenerateRandomString(string tokenKey)
    {
        int length = 64;

        var bytes = new byte[length];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes)
                      .TrimEnd('=')
                      .Replace('+', '-')
                      .Replace('/', '_');
    }
}