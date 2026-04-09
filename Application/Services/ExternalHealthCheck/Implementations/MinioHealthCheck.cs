using Application.Services.Blob.Minio;
using Application.Services.ExternalHealthCheck.Enums;
using Application.Services.ExternalHealthCheck.Interface;
using Application.Services.ExternalHealthCheck.Response;
using Loop.PersonalLogger;
using Microsoft.Extensions.Options;
using Minio;
using Shared.Results;
using Shared.Results.Errors.ExternalServices.Minio;


namespace Application.Services.ExternalHealthCheck.Implementations;

public class MinioHealthCheck : IExternalHealthCheck
{
    public string Name => "MinIO";
    public bool IsCritical => true;

    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;


    public MinioHealthCheck(IOptions<MinioSettings> options)
    {
        _bucketName = options.Value.Bucket;
        _minioClient = new MinioClient()
            .WithEndpoint(options.Value.Endpoint, options.Value.Port)
            .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
            .WithSSL(options.Value.UseSsl)
            .Build();
    }

    public async Task<Result<ExternalHealthResponse>> CheckAsync(CancellationToken ct)
    {
        PersonalLogger.Log($"Comenzando revision del servicio {Name}...");

        var externalHealthCheck = new ExternalHealthResponse
        {
            Name = Name,
            IsCritical = IsCritical,
            Status = ExternalServiceStatus.Healthy
        };

        // 🔹 Validar configuración
        if (string.IsNullOrWhiteSpace(_bucketName))
        {
            PersonalLogger.Log($"Error: Bucket de MinIO no configurado para el servicio {Name}.",LogType.Error);
            return Result<ExternalHealthResponse>.Failure(MinioError.BucketNotConfigured);
        }

        if (_minioClient is null)
        {
            PersonalLogger.Log($"Error: Cliente de MinIO no configurado para el servicio {Name}." ,LogType.Error);
            return Result<ExternalHealthResponse>.Failure(MinioError.ClientNotConfigured);
        }

        // 🔹 Validar conexión real
        try
        {
            PersonalLogger.Log("Validando conexión a MinIO...");
            var buckets = await _minioClient.ListBucketsAsync(ct);
            PersonalLogger.Log($"Buckets disponibles: {buckets.Buckets.Count}");

            var existBucket = buckets.Buckets.Any(b => b.Name == _bucketName);

            if (!existBucket)
            {
                PersonalLogger.Log($"Error: El Bucket '{_bucketName}' no existe en el servicio {Name}.", LogType.Error);
                return Result<ExternalHealthResponse>.Failure(MinioError.BucketDoesNotExist);
            }

            return Result<ExternalHealthResponse>.Success(externalHealthCheck);
        }
        catch (TimeoutException)
        {
            PersonalLogger.Log($"Error: Tiempo de espera agotado al intentar conectar con el servicio {Name}.", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(MinioError.MinioTimeout);
        }
        catch (Exception ex)
        {
            PersonalLogger.Log($"Error: No se pudo conectar con el servicio {Name}. Detalles: {ex.Message}", LogType.Error);
            return Result<ExternalHealthResponse>.Failure(MinioError.MinioConnectionFailed);
        }
    }
}