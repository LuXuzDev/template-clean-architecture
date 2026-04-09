using Application.Services.ExternalHealthCheck.Enums;
using Application.Services.ExternalHealthCheck.Implementations;
using Application.Services.ExternalHealthCheck.Response;
using FastEndpoints;
using Loop.PersonalLogger;
using Shared.Exception;
using Shared.Results;

namespace Application.Services.ExternalHealthCheck.Query;

public class ExternalHealthCheckQueryHandler : CommandHandler<ExternalHealthCheckQuery, Result<ExternalHealthReport>>
{
    private readonly ExternalHealthService _externalHealthCheck;

    public ExternalHealthCheckQueryHandler(ExternalHealthService externalHealthCheck)
    {
        _externalHealthCheck = externalHealthCheck;
    }

    public override async Task<Result<ExternalHealthReport>> ExecuteAsync(ExternalHealthCheckQuery command, CancellationToken ct = default)
    {
        var report = await _externalHealthCheck.CheckAllAsync(ct);

        foreach(var result in report.Value!.Services)
        {
            if(result.IsCritical && result.Status != ExternalServiceStatus.Healthy)
            {
                PersonalLogger.Log($"Servicio {result.Name} Estado: {result.Status}. Imposible continuar la ejecucion.", LogType.Error);

                if (result.Error != null && result.Error.HttpCode >= 500 &&
                    result.Error.Code.Contains("CONFIG"))
                {
                    ExceptionHelper.ThrowConfiguration(
                        $"Critical service {result.Name} failed. Status: {result.Status}. Detalle: {result.Error.Message}"
                    );
                }
                else
                {
                    ExceptionHelper.ThrowExternalService(
                        $"Critical service {result.Name} failed. Status: {result.Status}. Detalle: {result.Error?.Message ?? "No details"}"
                    );
                }
            }
            else if(!result.IsCritical && result.Status != ExternalServiceStatus.Healthy)
            {
                PersonalLogger.Log($"Servicio no crítico {result.Name} Estado: {result.Status}. Continuando con la ejecución.", LogType.Warning);
            }
            else
            {
                PersonalLogger.Log($"Servicio {result.Name} está saludable.", LogType.Success);
            }
        }
        return Result<ExternalHealthReport>.Success(report.Value!);
    }
}
