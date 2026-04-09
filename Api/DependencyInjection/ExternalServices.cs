using Application.Services.Blob;
using Application.Services.Blob.Minio;
using Application.Services.ExternalHealthCheck.Enums;
using Application.Services.ExternalHealthCheck.Implementations;
using Application.Services.ExternalHealthCheck.Interface;
using Application.Services.PersonalLoggerNotifier;
using Loop.PersonalLogger;
using Microsoft.Extensions.Options;
using Minio;
using Shared.Exception;

namespace Api.DependencyInjection;


public static class ExternalServices
{
    public static IServiceCollection AddExternalServices
      (this IServiceCollection services, IConfiguration configuration)
    {

        #region Minio
        services.Configure<MinioSettings>(configuration.GetSection(MinioSettings.SectionName));

        services.AddSingleton<IMinioClient>(provider =>
        {
            var minioOptions = provider.GetRequiredService<IOptions<MinioSettings>>().Value;

            // Configuracion del cliente MinIO
            var minioClient = new MinioClient()
                .WithEndpoint(minioOptions.Endpoint)
                .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey)
                .WithSSL(minioOptions.UseSsl)
                .Build();

            return minioClient;
        });

        services.AddScoped<IBlobService, MinioBlobService>();
        #endregion

        #region Health Checks

        services.AddSingleton<IExternalHealthCheck, PostgresHealthCheck>();
        services.AddSingleton<IExternalHealthCheck, MinioHealthCheck>();
        services.AddSingleton<ExternalHealthService>();

        #endregion

        return services;
    }


    public static async Task CheckExternalHealthAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var healthService = scope.ServiceProvider.GetRequiredService<ExternalHealthService>();
        var report = await healthService.CheckAllAsync(CancellationToken.None);

        foreach (var result in report.Value!.Services)
        {
            if (result.IsCritical && result.Status != ExternalServiceStatus.Healthy)
            {
                if (result.Error != null && result.Error.HttpCode >= 500 &&result.Error.Code.Contains("CONFIG"))
                {
                    PersonalLogger.Log(
                        $"[ERROR DE CONFIGURACIÓN] Servicio crítico {result.Name} falló. Estado: {result.Status}. Detalle: {result.Error.Message}",
                        LogType.Error,
                        PersonalLoggerName.Name
                    );

                    ExceptionHelper.ThrowConfiguration(
                        $"Critical service {result.Name} failed. Status: {result.Status}. Detalle: {result.Error.Message}"
                    );
                }
                else
                {
                    PersonalLogger.Log(
                        $"[ERROR DE SERVICIO EXTERNO] Servicio crítico {result.Name} falló. Estado: {result.Status}. Detalle: {result.Error?.Message ?? "Sin detalles"}",
                        LogType.Error,
                        PersonalLoggerName.Name
                    );

                    ExceptionHelper.ThrowExternalService(
                        $"Critical service {result.Name} failed. Status: {result.Status}. Detalle: {result.Error?.Message ?? "No details"}"
                    );
                }
            }
            else if (!result.IsCritical && result.Status != ExternalServiceStatus.Healthy)
            {
                PersonalLogger.Log($"Servicio no crítico {result.Name} Estado: {result.Status}. Continuando con la ejecución.", LogType.Warning, PersonalLoggerName.Name);
            }
            else
            {
                PersonalLogger.Log($"Servicio {result.Name} está saludable.", LogType.Success, PersonalLoggerName.Name);
            }
        }
    }
}
