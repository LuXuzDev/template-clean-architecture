using Microsoft.AspNetCore.Http;
using Shared.Results;


namespace Application.Services.Blob;


public interface IBlobService
{
    /// <summary>
    /// Sube un archivo al Blob, opcionalmente reemplazando uno previo. Retorna el string único Key generado.
    /// </summary>
    Task<Result<string>> UploadBlobAsync(IFormFile file, string? previousKey = null, CancellationToken ct = default);

    /// <summary>
    /// Sube un documentoal Blob, opcionalmente reemplazando uno previo. Retorna el string único Key generado.
    /// </summary>
    Task<Result<string>> UploadBlobDocumentAsync(IFormFile file, string? previousKey = null, CancellationToken ct = default);

    /// <summary>
    /// Sube un archivo al Blob dado su path local, útil para seeders o scripts.
    /// </summary>
    Task<Result<string>> UploadBlobFromPathAsync(string localFilePath, string? previousKey = null, CancellationToken ct = default);

    /// <summary>
    /// Obtiene la URL pre-firmada de un objeto dado su Key.
    /// </summary>
    Task<Result<string>> PresignedGetUrlAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Elimina un blob del Blob dado su Key.
    /// </summary>
    Task DeleteBlobAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Valida si un blob existe dado su Key.
    /// </summary>
    Task<Result<bool>> ValidateBlobExistenceAsync(string key, CancellationToken ct = default);
}
