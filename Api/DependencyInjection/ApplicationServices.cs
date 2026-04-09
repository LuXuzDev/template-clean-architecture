using Application.BackgroundServices.CleanupRefreshTokens;
using Application.Services.CurrentUser;
using Application.Services.DataProtector;
using Application.Services.Jwt;
using Application.Services.UserValidator;
using Domain.Entities.Users.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.Exception;
using Shared.Results;
using Shared.Results.Errors.Auth;
using System.Text;

namespace Api.DependencyInjection;

public static class ApplicationServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Configuración JWT con validación

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

        // ✅ Validación de configuración JWT
        if (jwtSettings == null)
            ExceptionHelper.ThrowConfiguration("JwtSettings no está configurado en appsettings.json");

        if (string.IsNullOrEmpty(JwtSettings.SecretKey) || JwtSettings.SecretKey.Length < 32)
            ExceptionHelper.ThrowConfiguration("Jwt SecretKey debe tener al menos 32 caracteres");
        #endregion


        #region Jwt

        services.AddScoped<IJwtServices, JwtServices>();
        services.AddHttpContextAccessor();

        #endregion


        #region Services

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserValidatorService, UserValidatorService>();

        #endregion


        #region Background Services

        services.AddHostedService<CleanupRefreshTokensService>();

        #endregion


        #region DataProtector

        services.AddDataProtection();
        services.AddScoped<IDataProtectorFactory, DataProtectorFactory>();

        // todo: descomentar DEPLOYMENT
        /*services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
            .SetApplicationName("MaryKeyApp");
        */

        services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(@"C:\DataProtectionKeys"))
            .SetApplicationName("TemplateApp");

        #endregion


        #region Cors
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", b =>
            {
                b.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        #endregion


        #region Configuración de Autenticación JWT

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtSettings.Issuer,
                    ValidAudience = JwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(JwtSettings.SecretKey)),
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async ctx =>
                    {
                        // Evita el 401 automático
                        ctx.HandleResponse();

                        var endpoint = ctx.HttpContext.GetEndpoint();

                        // Endpoints AllowAnonymous no se bloquean
                        if (endpoint?.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
                            return;

                        // Construye tu resultado genérico con AuthErrors.Unauthorized
                        var response = Result<object>.Failure(AuthError.Unauthorized);

                        ctx.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        ctx.HttpContext.Response.ContentType = "application/json";

                        await ctx.HttpContext.Response.WriteAsJsonAsync(response);
                    }
                };

            });


        #endregion


        #region Servicios de Autorizacion
        services.AddAuthorization();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        #endregion


        #region FastEndpoints
        services.AddFastEndpoints();
        #endregion

        return services;
    }
}

