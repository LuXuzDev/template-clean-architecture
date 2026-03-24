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
        Func<object, Task> sendBadRequest,
        Func<object, Task> sendOk,
        CancellationToken ct)
    {
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

            await sendBadRequest(new
            {
                code = "VALIDATION_ERROR",
                errors
            });
            return;
        }

        var result = await action();

        if (result.IsFailure)
        {
            await sendBadRequest(new
            {
                code = result.Error?.Code,
                message = result.Error?.Message
            });
            return;
        }

        await sendOk(result);
    }
}
