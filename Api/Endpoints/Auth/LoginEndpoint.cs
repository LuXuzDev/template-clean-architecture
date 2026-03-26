using Api.Endpoints.Helpers;
using Application.Features.Auth.Login;
using Application.Features.Auth.Shared.Response;
using FastEndpoints;
using Shared.Results;

namespace Api.Endpoints.Auth;

public class LoginEndpoint : Endpoint <LoginRequest, Result<AuthTokenResponse>>
{
    public override void Configure()
    {
        Post("/auth/login");
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Iniciar sesión";
            s.Description = "Autentica un usuario usando su nombre de usuario o correo electrónico y contraseña";
            s.ExampleRequest = new LoginRequest
            {
                Email = "usuario@example.com",
                Password = "Password123!"
            };
            s.Response<Result<AuthTokenResponse>> (200, "Autenticación exitosa");
            s.Response(400, "Datos inválidos");
            s.Response(401, "Credenciales incorrectas");
            s.Response(404, "Usuario no encontrado");
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        await EndpointHelper.HandleAsync(
        req,
        new LoginRequestValidator(),
        async () =>
        {
            var command = new LoginCommand { Request = req };
            return await command.ExecuteAsync(ct);
        },
       sendResponse: (obj, statusCode) => Send.ResultAsync(Results.Json(obj, statusCode: statusCode)),
        sendOk: obj => Send.OkAsync((Result<AuthTokenResponse>)obj),
        ct);
    }
}
