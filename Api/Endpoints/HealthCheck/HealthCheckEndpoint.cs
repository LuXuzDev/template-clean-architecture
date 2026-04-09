using Api.Endpoints.Helpers;
using Application.Features.Shared.Validators;
using Application.Services.ExternalHealthCheck.Query;
using Application.Services.ExternalHealthCheck.Response;
using FastEndpoints;
using Shared.Results;

namespace Api.Endpoints.HealthCheck;

public class HealthCheckEndpoint : EndpointWithoutRequest<Result<ExternalHealthReport>>
{
    public override void Configure()
    {
        Get("/health/external");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Chequeo de salud de servicios externos";
            s.Description = "Retorna el estado de todos los servicios externos configurados. No detiene la app si algún servicio está degradado, pero sí reporta errores críticos.";

            s.Response<Result<ExternalHealthReport>>(200, "Chequeo de salud completado");
            s.Response(500, "Error al realizar el chequeo de salud de los servicios externos");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await EndpointHelper.HandleAsync(
        new object(), // no request
        new NoValidator<object>(),
        async () =>
        {
            var command = new ExternalHealthCheckQuery ();
            return await command.ExecuteAsync(ct);
        },
        sendResponse: (obj, statusCode) => Send.ResultAsync(Results.Json(obj, statusCode: statusCode)),
        sendOk: obj => Send.OkAsync((Result<ExternalHealthReport>)obj),
        ct);
    }
}
