using Application.Services.Jwt;


namespace Application.Services.CurrentUser;

public interface ICurrentUserService
{
    /// <summary>
    /// Obtiene y valida la información del usuario autenticado a partir del contexto HTTP.
    /// </summary>
    /// <remarks>
    /// Este método:
    /// <list type="number">
    ///     <item>Comprueba si ya existe un usuario validado en caché para evitar múltiples lecturas.</item>
    ///     <item>Extrae los claims del token JWT presente en el contexto HTTP.</item>
    ///     <item>Verifica que el usuario exista realmente en base de datos.</item>
    /// </list>
    /// Si alguno de los pasos falla (sin contexto, token inválido o usuario no encontrado), 
    /// retorna <c>null</c>.
    /// </remarks>
    /// <param name="ct">
    /// Token de cancelación opcional para detener la operación asincrónica si es necesario.
    /// </param>
    /// <returns>
    /// Un <see cref="Task{TResult}"/> cuyo resultado es una instancia de <see cref="UserClaims"/> 
    /// con la información del usuario validado, o <c>null</c> si la validación no fue posible.
    /// </returns>
    public Task<UserClaims?> GetValidatedUserAsync(CancellationToken ct = default);
}
