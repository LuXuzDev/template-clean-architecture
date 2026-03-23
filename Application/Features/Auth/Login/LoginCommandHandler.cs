using Application.Features.Auth.Shared.Response;
using Application.Features.RefreshTokens.GenerateRefreshToken;
using Application.Helpers;
using Application.Services.Jwt;
using Domain.Users.Models;
using Domain.Users.Repository;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Shared.Results;
using Shared.Results.Errors.Auth;

namespace Application.Features.Auth.Login;

public class LoginCommandHandler : CommandHandler<LoginCommand, Result<AuthResponse>>
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
    public async override Task<Result<AuthResponse>> ExecuteAsync(LoginCommand command, CancellationToken ct = default)
    {
        var req = command.Request;


        var userEntity = await _userRepository!.GetByEmailAsync(HasherHelper.Hash(req.Email), ct);
        if (userEntity is null)
            return Result<AuthResponse>.Failure(AuthErrors.InvalidCredentials);


        var isPasswordCorrect = _passwordHasher.VerifyHashedPassword(null!, userEntity!.Password, req.Password);
        if (isPasswordCorrect == PasswordVerificationResult.Failed)
            return Result<AuthResponse>.Failure(AuthErrors.InvalidCredentials);


        var commandRf = new GenerateRefreshTokenCommand
        {
            Request = new GenerateRefreshTokenRequest
            {
                UserId = userEntity.Id,
            }
        };
        var refreshToken = await commandRf.ExecuteAsync(ct);

        if (refreshToken.IsFailure)
            return Result<AuthResponse>.Failure(refreshToken.Error!);


        return Result<AuthResponse>.Success(new AuthResponse
        {
            Token = _jwtServices.GenerateToken(userEntity),
            RefreshToken = refreshToken.Value!,
            ExpiresToken = JwtSettings.ExpirationInMinutes,
            ExpiresRefreshToken = JwtSettings.RefreshTokenExpirationInMinutes,
            Role = userEntity.Role!.Name
        });
    }
}

