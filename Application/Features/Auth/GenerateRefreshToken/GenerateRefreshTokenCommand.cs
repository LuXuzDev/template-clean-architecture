using FastEndpoints;
using Shared.Results;

namespace Application.Features.Auth.GenerateRefreshToken;

public class GenerateRefreshTokenCommand : ICommand<Result<string>>
{
    public GenerateRefreshTokenRequest Request { get; set; } = null!;
}
