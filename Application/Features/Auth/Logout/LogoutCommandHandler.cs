using Application.Helpers.Hasher;
using Application.Services.UserValidator;
using Domain.Entities.RefreshTokens.Models;
using Domain.Entities.RefreshTokens.Repository;
using Domain.Shared.Abstractions;
using Domain.Specifications.RefreshTokens;
using FastEndpoints;
using Shared.Results;
using Shared.Results.Succes.Auth;

namespace Application.Features.Auth.Logout;

public class LogoutCommandHandler : CommandHandler<LogoutCommand, Result<string>>
{
    private readonly ITokenBlackListRepository _tokenBlackListRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserValidatorService _userValidatorService;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler
        (ITokenBlackListRepository tokenBlackListRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IUserValidatorService userValidatorService,
        IUnitOfWork unitOfWork)
    {
        _tokenBlackListRepository = tokenBlackListRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _userValidatorService = userValidatorService;
        _unitOfWork = unitOfWork;
    }

    public override async Task<Result<string>> ExecuteAsync(LogoutCommand command, CancellationToken ct = default)
    {
        var req = command.Request;
        var result = await _userValidatorService.ValidateAsync(ct);
        
        if(result.IsFailure)
            return Result<string>.Failure(result.Error!);

        var tokenHash = HasherHelper.ComputeRefreshTokenHash(req.RefreshToken);

        await _refreshTokenRepository.DeleteBySpecAsync(new RefreshTokenByTokenSpecification(tokenHash), ct);

        var blacklistedToken = new TokenBlackList
        {
            UserId = result.Value!.UserId.ToString(),
            Token = req.Token
        };

        await _tokenBlackListRepository.CreateAsync(blacklistedToken);


        await _unitOfWork.SaveChangesAsync(ct);
        return Result<string>.Success(AuthSuccess.LogoutSuccess);
    }
}
