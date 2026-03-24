using Domain.Shared.Abstractions;


namespace Domain.RefreshTokens.Models;


/// <summary>
/// Representa un token de actualizacion (refresh token) asociado a un usuario.
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// Identificador del usuario al que pertenece el token.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Hash del token de actualizaci�n.
    /// </summary>
    public string TokenHash { get; set; } = null!;

    /// <summary>
    /// Fecha y hora en que expira el token.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Fecha y hora en que el token fue revocado, si aplica.
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Hash del token que reemplaza a este token, si fue reemplazado.
    /// </summary>
    public string? ReplacedByTokenHash { get; set; }
}
