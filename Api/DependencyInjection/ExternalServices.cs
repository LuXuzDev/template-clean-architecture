using Application.Services.Blob;
using Application.Services.Blob.Minio;
using LuxuzDev.PersonalLogger;
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


        return services;
    }

    /// <summary>
    /// Revisa la conectividad y disponibilidad del servicio MinIO al iniciar la aplicación.
    /// </summary>
    /// <param name="app">Instancia de la aplicación.</param>
    /// <returns></returns>
    public static async Task CheckMinioServiceAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();


        var blobService = scope.ServiceProvider.GetRequiredService<IBlobService>();

        if (blobService is MinioBlobService minioService)
        {
            var isConnected = await minioService.ValidateConnectionAsync();
            if (isConnected)
                PersonalLogger.Log("✅ MinIO está conectado y operativo.", LogType.Success);
            else
                ExceptionHelper.ThrowExternalService("❌ Minio no esta disponible");
        }
        else
        {
            PersonalLogger.Log("⚠️ No se encontró una implementación válida de MinioService.");
        }
    }
}
