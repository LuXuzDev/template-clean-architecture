using Application.Services.ExternalHealthCheck.Enums;
using Application.Services.ExternalHealthCheck.Interface;
using Application.Services.ExternalHealthCheck.Response;
using Shared.Results;


namespace Application.Services.ExternalHealthCheck.Implementations;

public class ExternalHealthService
{
    private readonly IEnumerable<IExternalHealthCheck> _checks;

    public ExternalHealthService(IEnumerable<IExternalHealthCheck> checks)
    {
        _checks = checks;
    }

    public async Task<Result<ExternalHealthReport>> CheckAllAsync(CancellationToken ct)
    {
        var results = new List<ExternalHealthResponse>();

        foreach (var check in _checks)
        {
            var result = await check.CheckAsync(ct);

            if (result.IsFailure)
            {
                results.Add(result.Value ?? new ExternalHealthResponse
                {
                    Name = check.Name,
                    IsCritical = check.IsCritical,
                    Status = ExternalServiceStatus.Unhealthy
                });
            }
            else
            {
                results.Add(result.Value!);
            }
        }

        var globalStatus = results.Any(r => r.IsCritical && r.Status == ExternalServiceStatus.Unhealthy)
            ? ExternalServiceStatus.Unhealthy
            : ExternalServiceStatus.Healthy;

        return Result<ExternalHealthReport>.Success(new ExternalHealthReport
        {
            Status = globalStatus,
            Services = results
        });
    }
}