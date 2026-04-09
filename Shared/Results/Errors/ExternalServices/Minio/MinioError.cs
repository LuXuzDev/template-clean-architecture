namespace Shared.Results.Errors.ExternalServices.Minio;

public static class MinioError
{
    public static readonly Error BucketNotConfigured =
        new("MINIO_BUCKET_NOT_CONFIGURED", "MinIO bucket not configured", 500);

    public static readonly Error ClientNotConfigured =
        new("MINIO_CLIENT_NOT_CONFIGURED", "MinIO client not configured", 500);

    public static readonly Error BucketDoesNotExist =
        new("MINIO_BUCKET_DOES_NOT_EXIST", "The specified bucket does not exist in MinIO", 500);

    public static readonly Error MinioTimeout =
        new("MINIO_TIMEOUT", "Timeout connecting to MinIO service", 504);

    public static readonly Error MinioConnectionFailed =
        new("MINIO_CONNECTION_FAILED", "Unable to connect to MinIO service", 500);
}