using FastEndpoints;
using Shared.Results;

namespace Application.Features.Auth.Logout;

public class LogoutCommand : ICommand<Result<string>>
{
    public LogoutRequest Request { get; set; } = null!;
}
