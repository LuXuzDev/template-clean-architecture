using Microsoft.AspNetCore.Http;


namespace Application.Helpers.Validators;

public static class FileValidator
{

    private static readonly string[] AllowedExtensions =
    {
        ".jpg", ".jpeg", ".png",
        ".mp4", ".mov", ".avi",
    };

    private static readonly string[] AllowedImageExtensions =
    {
        ".jpg", ".jpeg", ".png"
    };

    private static readonly string[] AllowedVideoExtensions =
    {
        ".mp4", ".mov", ".avi"
    };

    public static bool BeValidMediaType(IFormFile file)
    {
        if (file is null)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var isValidExtension = AllowedExtensions.Contains(extension);

        var isValidContentType =
            file.ContentType.StartsWith("image/")
            || file.ContentType.StartsWith("video/");

        return isValidExtension && isValidContentType;
    }

    public static bool BeValidImageType(IFormFile file)
    {
        if (file is null)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var isValidExtension = AllowedImageExtensions.Contains(extension);

        var isValidContentType = file.ContentType.StartsWith("image/");

        return isValidExtension && isValidContentType;
    }

    public static bool BeValidVideoType(IFormFile file)
    {
        if (file is null)
            return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var isValidExtension = AllowedVideoExtensions.Contains(extension);

        var isValidContentType = file.ContentType.StartsWith("video/");

        return isValidExtension && isValidContentType;
    }

    public static bool BeValidFileSize(IFormFile file, int sizeInMb)
    {
        if (file is null) return false;

        long maxSizeInBytes = sizeInMb * 1024 * 1024;
        return file.Length <= maxSizeInBytes;
    }
}
