using LuxuzDev.PersonalLogger;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Shared.Results;
using Shared.Results.Errors.MediaFile;


namespace Application.Services.Blob.Minio;

public class MinioBlobService : IBlobService
{
    private readonly string _bucketName;
    private readonly IMinioClient _minioClient;
    private readonly string _fileName = "images-template";

    public MinioBlobService(IOptions<MinioSettings> options)
    {
        _bucketName = options.Value.Bucket;
        _minioClient = new MinioClient()
            .WithEndpoint(options.Value.Endpoint)
            .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
            .WithSSL(options.Value.UseSsl)
            .Build();
    }

    private string GenerateUniqueKey(string extension)
    {
        // Genera un string único para Key + extensión
        return $"{Guid.NewGuid():N}{extension}";
    }

    public async Task<Result<string>> UploadBlobAsync(IFormFile? file, string? previousKey = null, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return Result<string>.Failure(MediaFileErros.InvalidFile);
            

        var ext = Path.GetExtension(file!.FileName).ToLower();
        var newKey = GenerateUniqueKey(ext);
        var objectPath = $"{_fileName}/{newKey}";

        // Eliminar previo si aplica
        if (!string.IsNullOrEmpty(previousKey))
            await DeleteBlobAsync(previousKey, ct);

        await using var stream = file.OpenReadStream();
        var putArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectPath)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(putArgs, ct);
        PersonalLogger.Log($"⬆ Archivo subido a MinIO: {objectPath} (Key: {newKey})", LogType.Success);

        return Result<string>.Success(newKey);
    }

    public async Task<Result<string>> UploadBlobDocumentAsync(IFormFile? file, string? previousKey = null, CancellationToken ct = default)
    {
        if (file is null || file.Length == 0)
            return Result<string>.Failure(MediaFileErros.InvalidFile);

        var ext = Path.GetExtension(file!.FileName).ToLower();
        var newKey = GenerateUniqueKey(ext);
        var objectPath = $"{_fileName}/{newKey}";

        if (!string.IsNullOrEmpty(previousKey))
            await DeleteBlobAsync(previousKey, ct);

        await using var stream = file.OpenReadStream();
        var putArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectPath)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(putArgs, ct);
        PersonalLogger.Log($"⬆ Documento subido a MinIO: {objectPath} (Key: {newKey})", LogType.Success);

        return Result<string>.Success(newKey); ;
    }

    public async Task<Result<string>> UploadBlobFromPathAsync(string localFilePath, string? previousKey = null, CancellationToken ct = default)
    {
        if (!File.Exists(localFilePath))
            return Result<string>.Failure(MediaFileErros.NotFound);

        var ext = Path.GetExtension(localFilePath).ToLower();
        var newKey = GenerateUniqueKey(ext);
        var objectPath = $"{_fileName}/{newKey}";

        if (!string.IsNullOrEmpty(previousKey))
            await DeleteBlobAsync(previousKey, ct);

        await using var stream = File.OpenRead(localFilePath);
        var putArgs = new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(objectPath)
            .WithStreamData(stream)
            .WithObjectSize(new FileInfo(localFilePath).Length)
            .WithContentType("application/octet-stream");

        await _minioClient.PutObjectAsync(putArgs, ct);
        PersonalLogger.Log($"⬆ Archivo subido desde path a MinIO: {objectPath} (Key: {newKey})", LogType.Success);

        return Result<string>.Success(newKey); ;
    }

    public async Task<Result<string>> PresignedGetUrlAsync(string key, CancellationToken ct = default)
    {
        var objectPath = $"{_fileName}/{key}";
        try
        {
            var args = new PresignedGetObjectArgs()
                .WithBucket(_bucketName)
                .WithObject(objectPath)
                .WithExpiry(3600);

            var url = await _minioClient.PresignedGetObjectAsync(args);
            return Result<string>.Success(url);
        }
        catch
        {
            return Result<string>.Failure(MediaFileErros.MinioPresignedUrlError);
        }
    }

    public async Task DeleteBlobAsync(string key, CancellationToken ct = default)
    {
        var objectPath = $"{_fileName}/{key}";
        var args = new RemoveObjectArgs().WithBucket(_bucketName).WithObject(objectPath);
        await _minioClient.RemoveObjectAsync(args, ct);
        PersonalLogger.Log($"🗑 Archivo eliminado de MinIO: {objectPath} (Key: {key})", LogType.Success);
    }

    public async Task<Result<bool>> ValidateBlobExistenceAsync(string key, CancellationToken ct = default)
    {
        var objectPath = $"{_fileName}/{key}";
        try
        {
            var args = new StatObjectArgs().WithBucket(_bucketName).WithObject(objectPath);
            await _minioClient.StatObjectAsync(args, ct);
            return Result<bool>.Success(true);
        }
        catch
        {
            return Result<bool>.Success(false);
        }
    }

    public async Task<bool> ValidateConnectionAsync(CancellationToken ct = default)
    {
        try
        {
            PersonalLogger.Log("🔄 Validando conexión a MinIO...");
            var buckets = await _minioClient.ListBucketsAsync(ct);
            PersonalLogger.Log($"📦 Buckets disponibles: {buckets.Buckets.Count}");

            foreach (var b in buckets.Buckets)
                PersonalLogger.Log($" - {b.Name}");

            return buckets.Buckets.Any(b => b.Name == _bucketName);
        }
        catch (Exception ex)
        {
            PersonalLogger.Log($"❌ Error validando conexión a MinIO: {ex.Message}", LogType.Error);
            return false;
        }
    }
}
