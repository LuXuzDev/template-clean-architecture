using Application.Services.ExternalHealthCheck.Response;
using FastEndpoints;
using Shared.Results;

namespace Application.Services.ExternalHealthCheck.Query;

public class ExternalHealthCheckQuery : ICommand<Result<ExternalHealthReport>>
{
}
