using Application.Services.Jwt;
using Domain.RefreshTokens.Repository;
using FastEndpoints;
using Shared.Results;
using System.Security.Cryptography;
using System.Text;


namespace Application.Features.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenCommandHandler : CommandHandler<GenerateRefreshTokenCommand, Result<string>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public GenerateRefreshTokenCommandHandler
        (IRefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async override Task<Result<string>> ExecuteAsync(GenerateRefreshTokenCommand command, CancellationToken ct = default)
    {
        var req = command.Request;
        using var sha256 = SHA256.Create();
        var refreshTokenString = GenerateRandomString(JwtSettings.SecretKey);
        var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshTokenString));
        DateTime created = DateTime.UtcNow;
        DateTime expired = DateTime.UtcNow.AddMinutes(JwtSettings.RefreshTokenExpirationInMinutes);

        var refreshTokenEntity = new Domain.RefreshTokens.Models.RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = req.UserId,
            TokenHash = Convert.ToBase64String(refreshTokenHash),
            CreatedAt = created,
            ExpiresAt = expired,
        };
        await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

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