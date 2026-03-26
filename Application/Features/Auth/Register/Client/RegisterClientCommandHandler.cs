using Application.Features.Auth.GenerateRefreshToken;
using Application.Features.Auth.Shared.Response;
using Application.Helpers;
using Application.Services.DataProtector;
using Application.Services.Jwt;
using Domain.Roles.Constants;
using Domain.Roles.Repository;
using Domain.Users.Models;
using Domain.Users.Repository;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Shared.Results;
using Shared.Results.Errors.Role;
using Shared.Results.Errors.User;

namespace Application.Features.Auth.Register.Client;

public class RegisterClientCommandHandler : CommandHandler<RegisterClientCommand , Result<AuthTokenResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtServices _jwtServices;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher<User> _passwordHasherService;
    private readonly IDataProtectorFactory _dataProtectorFactory;

    public RegisterClientCommandHandler
        (IUserRepository userRepository,
         IRoleRepository roleRepository,
         IPasswordHasher<User> passwordHasherService,
         IJwtServices jwtServices,
         IDataProtectorFactory dataProtectorFactory)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasherService = passwordHasherService;
        _jwtServices = jwtServices;
        _dataProtectorFactory = dataProtectorFactory;
    }

    public override async Task<Result<AuthTokenResponse>> ExecuteAsync(RegisterClientCommand command, CancellationToken ct)
    {
        var req = command.Request;
        var protectorEmail = _dataProtectorFactory.Create(DataPorpuse.UserEmail);


        var existingUserByEmail = await _userRepository.GetByEmailAsync(HasherHelper.Hash(req.Email), ct);

        if (existingUserByEmail is not null)
            return Result<AuthTokenResponse>.Failure(UserErrors.EmailInUse);

        var role = await _roleRepository.GetByNameAsync(RoleConstants.Client, ct);

        if (role is null)
            return Result<AuthTokenResponse>.Failure(RoleErrors.NotFound);


        var user = new User
        {
            EncryptedEmail = protectorEmail.Protect(req.Email),
            HashedEmail = HasherHelper.Hash(req.Email),
            Password = req.Password,
            RoleId = role!.Id,
        };

        user.Password = _passwordHasherService.HashPassword(user, req.Password);

        await _userRepository.CreateAsync(user);


        var commandRf = new GenerateRefreshTokenCommand
        {
            Request = new GenerateRefreshTokenRequest
            {
                UserId = user.Id,
            }
        };

        var refreshTokenResult = await commandRf.ExecuteAsync(ct);


        return Result<AuthTokenResponse>.Success(new AuthTokenResponse
        {
            Token = _jwtServices.GenerateToken(user),
            RefreshToken = refreshTokenResult.Value!,
            ExpiresToken = JwtSettings.ExpirationInMinutes,
            ExpiresRefreshToken = JwtSettings.RefreshTokenExpirationInMinutes,
            Role = user.Role!.Name,
        });
    }
}
