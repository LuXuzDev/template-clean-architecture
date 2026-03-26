using Api.Endpoints.Helpers;
using Application.Features.Auth.Logout;
using FastEndpoints;
using Shared.Results;

namespace Api.Endpoints.Auth;

public class LogoutEnpoint : Endpoint<LogoutRequest, Result<string>>
{
    public override void Configure()
    {
        Post("/auth/logout");

        Summary(s =>
        {
            s.Summary = "Cerrar sesión";
            s.Description = "Cierra la sesión de un usuario eliminando su refresh token y agregando el access token a la blacklist.";
            s.ExampleRequest = new LogoutRequest
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            };
            s.Response<Result<string>>(200, "Logout exitoso");
            s.Response(400, "Datos inválidos");
            s.Response(401, "Usuario no autorizado");
            s.Response(404, "Usuario no encontrado");
        });
    }

    public override async Task HandleAsync(LogoutRequest req, CancellationToken ct)
    {
        await EndpointHelper.HandleAsync(
        req,
        new LogoutRequestValidator(),
        async () =>
        {
            var command = new LogoutCommand { Request = req };
            return await command.ExecuteAsync(ct);
        },
        sendBadRequest: obj => Send.ResultAsync(Results.BadRequest(obj)),
        sendOk: obj => Send.OkAsync((Result<string>)obj),
        ct);
    }
}
