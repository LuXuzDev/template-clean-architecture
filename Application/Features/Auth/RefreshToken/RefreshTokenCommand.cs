using Application.Features.Auth.Shared.Response;
using FastEndpoints;
using Shared.Results;

namespace Application.Features.Auth.RefreshToken;

public class RefreshTokenCommand : ICommand<Result<AuthTokenResponse>>
{
    public RefreshTokenRequest Request { get; set; } = null!;
}
