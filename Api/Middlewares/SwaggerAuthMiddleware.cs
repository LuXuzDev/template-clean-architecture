using System.Text;

namespace Api.Middlewares;


public class SwaggerAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _config;

    public SwaggerAuthMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            string? authHeader = context.Request.Headers["Authorization"];

            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                var encodedUsernamePassword = authHeader["Basic ".Length..].Trim();
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                var parts = decoded.Split(':', 2);

                var expectedUser = _config["SwaggerAuth:Username"];
                var expectedPass = _config["SwaggerAuth:Password"];

                if (parts[0] == expectedUser && parts[1] == expectedPass)
                {
                    await _next(context);
                    return;
                }
            }

            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        await _next(context);
    }
}