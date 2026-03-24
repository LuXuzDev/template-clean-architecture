using Api.Endpoints.Helpers;
using Application.Features.Auth.RefreshToken;
using Application.Features.Auth.Shared.Response;
using FastEndpoints;
using Shared.Results;
using Shared.Results.Errors;

namespace Api.Endpoints.Auth;

public class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, Result<AuthTokenResponse>>
{
    public override void Configure()
    {
        Post("/auth/refresh");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Refrescar token de autenticación";
            s.Description = "Genera un nuevo token de acceso y un nuevo refresh token a partir de un refresh token válido";
            s.ExampleRequest = new RefreshTokenRequest
            {
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"
            };
            s.Response<Result<AuthTokenResponse>> (200, "Token renovado correctamente");
            s.Response(400, "Datos inválidos");
            s.Response(401, "Refresh token inválido o expirado");
            s.Response(404, "Usuario no encontrado");
        });
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        await EndpointHelper.HandleAsync(
        req,
        new RefreshTokenRequestValidator(),
        async () =>
        {
            var command = new RefreshTokenCommand { Request = req };
            return await command.ExecuteAsync(ct);
        },
        sendBadRequest: obj => Send.ResultAsync(Results.BadRequest(obj)),
        sendOk: obj => Send.OkAsync((Result<AuthTokenResponse>)obj),
        ct);
    }
}
