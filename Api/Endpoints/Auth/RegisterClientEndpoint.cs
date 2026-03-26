using Api.Endpoints.Helpers;
using Application.Features.Auth.Register.Client;
using Application.Features.Auth.Shared.Response;
using FastEndpoints;
using Shared.Results;

namespace Api.Endpoints.Auth;

public class RegisterClientEndpoint : Endpoint<RegisterClientRequest, Result<AuthTokenResponse>>
{
    public override void Configure()
    {
        Post("/auth/register");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Registrar cliente";
            s.Description = "Registra un nuevo cliente en el sistema utilizando email y contraseña";

            s.ExampleRequest = new RegisterClientRequest
            {
                Email = "usuario@example.com",
                Password = "Password123!"
            };

            s.Response<Result<AuthTokenResponse>>(200, "Cliente registrado exitosamente");
            s.Response(400, "Datos inválidos");
            s.Response(409, "El email ya está en uso");
        });
    }

    public override async Task HandleAsync(RegisterClientRequest req, CancellationToken ct)
    {
        await EndpointHelper.HandleAsync(
        req,
        new RegisterClientRequestValidator(),
        async () =>
        {
            var command = new RegisterClientCommand { Request = req };
            return await command.ExecuteAsync(ct);
        },
        sendResponse: (obj, statusCode) => Send.ResultAsync(Results.Json(obj, statusCode: statusCode)),
        sendOk: obj => Send.OkAsync((Result<AuthTokenResponse>)obj),
        ct);
    }
}
