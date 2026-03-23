namespace Shared.Results.Errors.MediaFile;

public static class MediaFileErros
{
    public static readonly Error InvalidFile =
        new("INVALID_FILE", "File not valid", 400);

    public static readonly Error NotFound =
        new("FILE_NOT_FOUND", "File not found", 404);

    public static readonly Error MinioPresignedUrlError =
        new("MINIO_URL_GENERATION_ERROR", "Error generating presigned URL", 500);

    public static readonly Error MinIOConnectionError =
        new("MINIO_CONNECTION_ERROR", "Unable to connect to MinIO storage service", 503);
}
