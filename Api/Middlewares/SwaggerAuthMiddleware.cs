using Application;

namespace Api.Middlewares;

public class SwaggerAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _password;

    public SwaggerAuthMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next = next;
        _password = config["SwaggerSettings:AccessPassword"]!;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Solo proteger rutas que empiecen con /swagger
        if (context.Request.Path.StartsWithSegments("/swagger"))
        {
            var authCookie = context.Request.Cookies["swagger-auth-" + GlobalAppInfo.Name];
            var isAuthenticated = authCookie == _password;

            if (!isAuthenticated)
            {
                // Si es un POST, procesar login
                if (context.Request.Method == "POST")
                {
                    await HandleLoginAsync(context);
                    return;
                }

                // Mostrar formulario de login
                await ShowLoginFormAsync(context);
                return;
            }
        }

        // Si está autenticado o no es ruta de swagger, continuar
        await _next(context);
    }

    private async Task HandleLoginAsync(HttpContext context)
    {
        var form = await context.Request.ReadFormAsync();
        var password = form["password"].ToString();

        if (password == _password)
        {
            context.Response.Cookies.Append("swagger-auth-" + GlobalAppInfo.Name, password, new CookieOptions
            {
                HttpOnly = true,
                Secure = context.Request.IsHttps,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(24)
            });

            context.Response.Redirect("/swagger");
        }
        else
        {
            context.Response.StatusCode = 401;
            await ShowLoginFormAsync(context, "Contraseña incorrecta");
        }
    }

    private static async Task ShowLoginFormAsync(HttpContext context, string? error = null)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "text/html";

        var errorHtml = error != null
            ? $"<div class='error'>{error}</div>"
            : "";

        var html = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Acceso Restringido - Swagger</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}

        body {{
            font-family: 'Segoe UI', -apple-system, BlinkMacSystemFont, sans-serif;
            background-color: #fafafa;
            color: #333333;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            padding: 20px;
        }}

        .login-box {{
            width: 100%;
            max-width: 420px;
            padding: 48px 40px;
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            background-color: #ffffff;
            text-align: center;
            transition: border-color 0.3s ease, box-shadow 0.3s ease;
        }}

        .login-box:hover {{
            border-color: #89bf04;
            box-shadow: 0 4px 12px rgba(137, 191, 4, 0.1);
        }}

        h2 {{
            font-size: 22px;
            font-weight: 600;
            letter-spacing: 0.5px;
            margin-bottom: 12px;
            color: #3b4151;
        }}

        p {{
            font-size: 14px;
            color: #6b6b6b;
            margin-bottom: 32px;
            line-height: 1.5;
        }}

        .error {{
            color: #d63e04;
            font-size: 13px;
            margin-bottom: 24px;
            padding: 12px;
            background-color: #fef5f0;
            border: 1px solid #f5d5c7;
            border-radius: 4px;
            letter-spacing: 0.3px;
        }}

        input[type='password'] {{
            width: 100%;
            padding: 14px 16px;
            margin-bottom: 20px;
            background-color: #ffffff;
            border: 1px solid #d9d9d9;
            border-radius: 4px;
            color: #3b4151;
            font-size: 15px;
            letter-spacing: 0.5px;
            outline: none;
            transition: border-color 0.2s ease, box-shadow 0.2s ease;
        }}

        input[type='password']::placeholder {{
            color: #999999;
        }}

        input[type='password']:focus {{
            border-color: #89bf04;
            box-shadow: 0 0 0 3px rgba(137, 191, 4, 0.1);
        }}

        button {{
            width: 100%;
            padding: 14px;
            background-color: #89bf04;
            color: #ffffff;
            border: none;
            border-radius: 4px;
            font-size: 14px;
            font-weight: 600;
            letter-spacing: 1px;
            text-transform: uppercase;
            cursor: pointer;
            transition: background-color 0.2s ease, transform 0.1s ease;
        }}

        button:hover {{
            background-color: #7ab004;
        }}

        button:active {{
            transform: scale(0.98);
            background-color: #6a9a03;
        }}

        .footer {{
            margin-top: 32px;
            font-size: 11px;
            color: #999999;
            letter-spacing: 0.5px;
        }}
    </style>
</head>
<body>
    <div class='login-box'>
        <h2>ACCESO RESTRINGIDO</h2>
        <p>Introduce la contraseña para acceder a la documentación Swagger</p>
        {errorHtml}
        <form method='POST' action='/swagger'>
            <input 
                type='password' 
                name='password' 
                placeholder='Contraseña' 
                required 
                autofocus 
                autocomplete='current-password'
            />
            <button type='submit'>Entrar</button>
        </form>
        <div class='footer'>DOCUMENTACIÓN PRIVADA</div>
    </div>
</body>
</html>";

        await context.Response.WriteAsync(html);
    }
}