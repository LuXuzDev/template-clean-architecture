namespace Application.Services.Blob.Minio;

public class MinioSettings
{
    public const string SectionName = "Minio";

    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string Bucket { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = false;
    public int Port { get; set; }
}
