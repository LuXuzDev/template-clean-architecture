using FluentValidation;
using Shared.Results;
using Shared.Results.Errors;

namespace Api.Endpoints.Helpers;

public static class EndpointHelper
{
    public static async Task HandleAsync<TRequest, TResult>(
    TRequest req,
    IValidator<TRequest> validator,
    Func<Task<Result<TResult>>> action,
    Func<object, int, Task> sendResponse, // ahora incluimos el status code
    Func<object, Task> sendOk,
    CancellationToken ct)
    {
        // 1️⃣ Validación
        var validationResult = await validator.ValidateAsync(req, ct);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => e.CustomState as ValidationError ?? new ValidationError(
                    Code: "VALIDATION_ERROR",
                    Message: e.ErrorMessage,
                    PropertyName: e.PropertyName
                ))
                .ToList();

            await sendResponse(new
            {
                code = "VALIDATION_ERROR",
                errors
            }, 400); // BadRequest
            return;
        }

        // 2️⃣ Ejecuta acción
        var result = await action();

        if (result.IsFailure)
        {
            // Usa el httpCode del error si existe, sino 400
            var statusCode = result.Error?.HttpCode ?? 400;

            await sendResponse(new
            {
                code = result.Error?.Code,
                message = result.Error?.Message,
                httpCode = result.Error?.HttpCode
            }, statusCode);

            return;
        }

        // 3️⃣ Éxito
        await sendOk(result);
    }
}
