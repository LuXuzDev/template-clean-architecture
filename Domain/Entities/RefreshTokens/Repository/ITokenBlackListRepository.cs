using Domain.Entities.RefreshTokens.Models;

namespace Domain.Entities.RefreshTokens.Repository;

public interface ITokenBlackListRepository
{
    /// <summary>
    /// Crea un nuevo registro en la lista negra de tokens (TokenBlackList).
    /// </summary>
    /// <param name="tokenBlackList">Entidad que representa el token que se desea agregar a la blacklist.</param>
    /// <param name="ct">Token de cancelación para abortar la operación asíncrona.</param>
    /// <returns>Una tarea que representa la operación de creación asincrónica.</returns>
    Task CreateAsync(TokenBlackList tokenBlackList, CancellationToken ct = default);


    /// <summary>
    /// Elimina todos los tokens de la lista negra que hayan sido creados hace más de un tiempo específico.
    /// </summary>
    /// <param name="expirationInMinutes">Cantidad de minutos que determina la antigüedad máxima de los tokens permitidos.</param>
    /// <param name="ct">Token de cancelación para abortar la operación asíncrona.</param>
    /// <returns>
    /// Una tarea que representa la operación asíncrona.
    /// El resultado de la tarea es el número de tokens eliminados.
    /// </returns>
    Task<int> DeleteExpiredAsync(int expirationInMinutes, CancellationToken ct = default);


    /// <summary>
    /// Verifica si un token específico se encuentra en la lista negra (TokenBlackList).
    /// </summary>
    /// <param name="token">El access token que se desea verificar.</param>
    /// <param name="ct">Token de cancelación para abortar la operación asíncrona.</param>
    /// <returns>Devuelve <c>true</c> si el token está en la blacklist, <c>false</c> en caso contrario.</returns>
    Task<bool> ExistsAsync(string token, CancellationToken ct = default);
}
