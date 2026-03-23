namespace Domain.Exceptions;

public enum ExceptionCode
{
    None,

    #region Autenticación

    /// <summary>
    /// Credenciales proporcionadas inválidas (usuario o contraseña incorrectos).
    /// </summary>
    InvalidCredentials,

    /// <summary>
    /// El token proporcionado ha expirado.
    /// </summary>
    TokenExpired,

    #endregion


    #region Imagenes/Blob
    /// <summary>
    /// Archivo no valido
    /// </summary>
    InvalidFile,

    /// <summary>
    /// Archivo no encontrado
    /// </summary>
    FileNotFound,
    #endregion


    #region Usuarios

    /// <summary>
    /// No se encontró el usuario solicitado en el sistema.
    /// </summary>
    UserNotFound,

    /// <summary>
    /// El correo electrónico proporcionado ya está registrado en el sistema.
    /// </summary>
    EmailAlreadyExists,

    /// <summary>
    /// El numero de telefono proporcionado ya está registrado en el sistema.
    /// </summary>
    PhoneAlreadyExits,

    #endregion


    #region Roles

    /// <summary>
    /// No se encontró el rol solicitado en el sistema.
    /// </summary>
    RoleNotFound,

    #endregion

    #region General

    /// <summary>
    /// Error de validacion en los datos proporcionados.
    /// </summary>
    ValidationError

    #endregion
}