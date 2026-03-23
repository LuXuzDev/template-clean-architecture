using Application.Features.Auth.Shared.Response;
using FastEndpoints;
using Shared.Results;

namespace Application.Features.Auth.Login;

public class LoginCommand : ICommand<Result<AuthResponse>>
{
    public LoginRequest Request { get; set; } = null!;
}
