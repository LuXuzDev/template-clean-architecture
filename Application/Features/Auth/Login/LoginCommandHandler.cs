using Application.Features.Auth.GenerateRefreshToken;
using Application.Features.Auth.Shared.Response;
using Application.Helpers;
using Application.Services.Jwt;
using Domain.Entities.Users.Models;
using Domain.Entities.Users.Repository;
using Domain.Specifications.Users;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Shared.Results;
using Shared.Results.Errors.Auth;

namespace Application.Features.Auth.Login;

public class LoginCommandHandler : CommandHandler<LoginCommand, Result<AuthTokenResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtServices _jwtServices;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginCommandHandler
        (IUserRepository userRepository,
        IJwtServices jwtServices,
        IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _jwtServices = jwtServices;
        _passwordHasher = passwordHasher;
    }
    public async override Task<Result<AuthTokenResponse>> ExecuteAsync(LoginCommand command, CancellationToken ct = default)
    {
        var req = command.Request;

        var userEntity = await _userRepository.FirstOrDefaultAsync(new UserByEmailSpecification(HasherHelper.Hash(req.Email)), ct);

        if (userEntity is null)
            return Result<AuthTokenResponse>.Failure(AuthErrors.InvalidCredentials);

        var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(null!, userEntity!.Password, req.Password);
        if (isPasswordCorrect == PasswordVerificationResult.Failed)
            return Result<AuthTokenResponse>.Failure(AuthErrors.InvalidCredentials);


        var commandRf = new GenerateRefreshTokenCommand
        {
            Request = new GenerateRefreshTokenRequest
            {
                UserId = userEntity.Id,
            }
        };
        var refreshToken = await commandRf.ExecuteAsync(ct);

        if (refreshToken.IsFailure)
            return Result<AuthTokenResponse>.Failure(refreshToken.Error!);


        return Result<AuthTokenResponse>.Success(new AuthTokenResponse
        {
            Token = _jwtServices.GenerateToken(userEntity),
            RefreshToken = refreshToken.Value!,
            ExpiresToken = JwtSettings.ExpirationInMinutes,
            ExpiresRefreshToken = JwtSettings.RefreshTokenExpirationInMinutes,
            Role = userEntity.Role!.Name
        });
    }
}

