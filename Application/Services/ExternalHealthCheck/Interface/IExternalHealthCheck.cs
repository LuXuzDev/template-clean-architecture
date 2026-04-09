using Application.Services.ExternalHealthCheck.Response;
using Shared.Results;


namespace Application.Services.ExternalHealthCheck.Interface;

public interface IExternalHealthCheck
{
    string Name { get; }
    bool IsCritical { get; }

    Task<Result<ExternalHealthResponse>> CheckAsync(CancellationToken ct);
}