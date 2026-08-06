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
        Func<object, int, Task> sendResponse,
        Func<object, Task> sendOk,
        CancellationToken ct)
    {
        // 1️. Validación de FluentValidation
        var validationResult = await validator.ValidateAsync(req, ct);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult.Errors
                .Select(e => e.CustomState as ValidationError ?? new ValidationError(
                    Code: "VALIDATION_ERROR",
                    Message: e.ErrorMessage,
                    PropertyName: e.PropertyName
                ))
                .ToList();

            // Creamos un Result de falla usando tu propia clase
            var resultFailure = Result<TResult>.Failure(validationErrors);

            // Enviamos el objeto Result completo
            await sendResponse(resultFailure, 400);
            return;
        }

        // 2️. Ejecución de la lógica (Command/Query)
        var result = await action();

        if (result.IsFailure)
        {
            // Extraemos el código HTTP del primer error si existe
            var statusCode = result.Errors.FirstOrDefault()?.HttpCode ?? 400;

            // ENVIAMOS EL OBJETO RESULT COMPLETO
            // Esto devolverá: isSuccess, isFailure, errors y value
            await sendResponse(result, statusCode);
            return;
        }

        // 3. Éxito
        // Aquí result.IsSuccess ya es true y result.IsFailure es false
        await sendOk(result);
    }
}
