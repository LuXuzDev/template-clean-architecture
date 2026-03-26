using Application.Features.Auth.Shared.Response;
using FastEndpoints;
using Shared.Results;

namespace Application.Features.Auth.Register.Client;

public class RegisterClientCommand : ICommand<Result<AuthTokenResponse>>
{
    public RegisterClientRequest Request { get; set; } = null!;
}
