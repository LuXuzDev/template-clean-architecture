namespace Shared.Results.Errors.Auth;


public static class AuthErrors
{
    /// <summary>
    /// Usuario no autorizado (sin token o token inválido)
    /// </summary>
    public static readonly Error Unauthorized =
        new("UNAUTHORIZED", "User is not authorized", 401);

    /// <summary>
    /// Acceso prohibido (usuario válido pero no tiene permisos)
    /// </summary>
    public static readonly Error Forbidden =
        new("FORBIDDEN", "Access is forbidden", 403);

    /// <summary>
    /// Credenciales incorrectas
    /// </summary>
    public static readonly Error InvalidCredentials =
        new("INVALID_CREDENTIALS", "The email or password is incorrect", 400);

    /// <summary>
    /// Token expirado
    /// </summary>
    public static readonly Error TokenExpired =
        new("TOKEN_EXPIRED", "The authentication token has expired", 401);

    /// <summary>
    /// Email es obligatorio
    /// </summary>
    public static readonly Error EmailRequired =
        new("EMAIL_REQUIRED", "Email is required", 400);

    /// <summary>
    /// Password es obligatorio
    /// </summary>
    public static readonly Error PasswordRequired =
        new("PASSWORD_REQUIRED", "Password is required", 400);

    /// <summary>
    /// Formato de correo inválido
    /// </summary>
    public static readonly Error InvalidEmailFormat =
        new("INVALID_EMAIL_FORMAT", "The email format is invalid", 400);
}