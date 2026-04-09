using Application.Services.PersonalLoggerNotifier;
using Domain.Exceptions;
using Loop.PersonalLogger;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        object response;
        string logMessage;

        if (exception is ApiException apiException)
        {
            statusCode = (int)apiException.ExceptionType;

            response = new
            {
                status = statusCode,
                message = apiException.ErrorMessage,
                details = apiException.ErrorDetails,
                date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            logMessage = $@"
                [API ERROR]
                Status: {statusCode}
                Message: {apiException.ErrorMessage}
                Details: {apiException.ErrorDetails}
                Path: {context.Request.Path}
                Method: {context.Request.Method}
                Date: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
                ";
        }
        else
        {
            statusCode = StatusCodes.Status500InternalServerError;

            response = new
            {
                status = statusCode,
                code = "UNEXPECTED_ERROR",
                message = "Ocurrió un error inesperado",
                details = exception.Message,
                date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            logMessage = $@"
                [UNHANDLED ERROR]
                Type: {exception.GetType().Name}
                Message: {exception.Message}
                Path: {context.Request.Path}
                Method: {context.Request.Method}
                TraceId: {context.TraceIdentifier}
                Date: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}
                ";
        }

        PersonalLogger.Log(logMessage, LogType.Error, PersonalLoggerName.Name);

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(response);
    }
}

