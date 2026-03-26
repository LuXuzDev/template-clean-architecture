using Domain.Entities.Roles.Models;
using Domain.Shared.Abstractions;


namespace Domain.Entities.Users.Models;

public class User : BaseEntity
{
    /// <summary>
    /// Correo electrónico encriptado del usuario, utilizado para retornar valores.
    /// </summary>
    public required string EncryptedEmail { get; set; }

    /// <summary>
    /// Correo electrónico hasheado del usuario, utilizado para búsquedas y validaciones.
    /// </summary>
    public required string HashedEmail { get; set; }

    /// <summary>
    /// Contraseña hasheada del usuario, almacenada de forma segura en la base de datos.
    /// </summary>
    public required string Password { get; set; }

    /// <summary>
    /// Identificador único del rol asignado al usuario.
    /// </summary>
    public required Guid RoleId { get; set; }

    /// <summary>
    /// Navegación al rol asociado del usuario. Puede ser nulo si el rol no está cargado.
    /// </summary>
    public Role? Role { get; set; }
}
