using Application.Services.Jwt;
using Shared.Results;

namespace Application.Services.UserValidator;


public interface IUserValidatorService
{
    /// <summary>
    /// Valida el usuario autenticado actual y, opcionalmente,
    /// verifica que posea al menos uno de los roles requeridos.
    /// </summary>
    /// <param name="ct">
    /// Token de cancelación utilizado para cancelar la operación asíncrona.
    /// </param>
    /// <param name="roles">
    /// Lista opcional de roles que el usuario debe tener.
    /// Si no se especifican roles, solo se valida que el usuario esté autenticado.
    /// </param>
    /// <returns>
    /// Devuelve los <see cref="UserClaims"/> del usuario validado.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Se lanza cuando el usuario no está autenticado.
    /// </exception>
    /// <exception cref="ForbiddenAccessException">
    /// Se lanza cuando el usuario no posee ninguno de los roles requeridos.
    /// </exception>
    Task<Result<UserClaims>> ValidateAsync(CancellationToken ct, params string[] roles);
}
