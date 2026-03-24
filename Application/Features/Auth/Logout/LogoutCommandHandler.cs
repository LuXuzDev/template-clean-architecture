using Application.Services.UserValidator;
using Domain.RefreshTokens.Models;
using Domain.RefreshTokens.Repository;
using FastEndpoints;
using Shared.Results;
using Shared.Results.Succes.Auth;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Auth.Logout;

public class LogoutCommandHandler : CommandHandler<LogoutCommand, Result<string>>
{
    private readonly ITokenBlackListRepository _tokenBlackListRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserValidatorService _userValidatorService;

    public LogoutCommandHandler
        (ITokenBlackListRepository tokenBlackListRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUserValidatorService userValidatorService)
    {
        _tokenBlackListRepository = tokenBlackListRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _userValidatorService = userValidatorService;
    }

    public override async Task<Result<string>> ExecuteAsync(LogoutCommand command, CancellationToken ct = default)
    {
        var req = command.Request;
        var result = await _userValidatorService.ValidateAsync(ct);
        
        if(result.IsFailure)
            return Result<string>.Failure(result.Error!);

        var tokenHash = ComputeRefreshTokenHash(req.RefreshToken);
        await _refreshTokenRepository.DeleteAsync(tokenHash, ct);

        var blacklistedToken = new TokenBlackList
        {
            UserId = result.Value!.UserId.ToString(),
            Token = req.Token
        };

        await _tokenBlackListRepository.CreateAsync(blacklistedToken);

        return Result<string>.Success(AuthSuccess.LogoutSuccess);
    }

    private string ComputeRefreshTokenHash(string refreshToken)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToBase64String(hashBytes);
    }
}
