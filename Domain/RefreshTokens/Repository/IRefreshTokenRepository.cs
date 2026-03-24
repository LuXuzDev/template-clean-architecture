using Domain.RefreshTokens.Models;

namespace Domain.RefreshTokens.Repository;

public interface IRefreshTokenRepository
{
    /// <summary>
    /// Crea un nuevo refresh token en el almacenamiento.
    /// </summary>
    /// <param name="refreshToken">La entidad de refresh token a crear.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task CreateAsync(RefreshToken refreshToken);

    /// <summary>
    /// Obtiene un refresh token a partir de su valor.
    /// </summary>
    /// <param name="refreshToken">El valor del refresh token.</param>
    /// <param name="ct">Token de cancelación para cancelar la operación.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// El resultado contiene el refresh token si existe; de lo contrario, null.
    /// </returns>
    Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default);

    /// <summary>
    /// Actualiza un refresh token existente en el almacenamiento.
    /// </summary>
    /// <param name="refreshToken">La entidad de refresh token a actualizar.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task UpdateAsync(RefreshToken refreshToken);

    /// <summary>
    /// Elimina todos los refresh tokens que hayan sido revocados o que hayan expirado.
    /// </summary>
    /// <param name="ct">Token de cancelación para abortar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// El resultado de la tarea es el número de tokens eliminados de la base de datos.
    /// </returns>
    Task<int> DeleteExpiredOrRevokedAsync(CancellationToken ct = default);

    /// <summary>
    /// Elimina un refresh token específico de la base de datos.
    /// </summary>
    /// <param name="hashedRefreshToken">El refresh token (hasheado) que se desea eliminar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de eliminación asíncrona.</returns>
    Task DeleteAsync(string hashedRefreshToken, CancellationToken ct = default);
}
