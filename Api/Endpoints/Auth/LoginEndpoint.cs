using Application.Features.Auth.Login;
using Application.Features.Auth.Shared.Response;
using FastEndpoints;
using Shared.Results;
using Shared.Results.Errors;

namespace Api.Endpoints.Auth;

public class LoginEndpoint : Endpoint <LoginRequest, Result<AuthResponse>>
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
            s.Response<Result<AuthResponse>> (200, "Autenticación exitosa");
            s.Response(400, "Datos inválidos");
            s.Response(401, "Credenciales incorrectas");
            s.Response(404, "Usuario no encontrado");
        });
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var validationResult = await new LoginRequestValidator().ValidateAsync(req, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => e.CustomState as ValidationError ?? new ValidationError(
                    Code: "VALIDATION_ERROR",
                    Message: e.ErrorMessage,
                    PropertyName: e.PropertyName
                ))
                .ToList();

            await Send.ResultAsync(Results.BadRequest(new
            {
                code = "VALIDATION_ERROR",
                errors
            }));
            return;
        }

        var command = new LoginCommand { Request = req };

        var result = await command.ExecuteAsync(ct);

        if (result.IsFailure)
        {
            await Send.ResultAsync(Results.BadRequest(new
            {
                code = result.Error?.Code,
                message = result.Error?.Message
            }));
            return;
        }

        await Send.OkAsync(result);
    }
}
