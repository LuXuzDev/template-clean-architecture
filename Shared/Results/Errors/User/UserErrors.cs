namespace Shared.Results.Errors.User;

public static class UserErrors
{
    /// <summary>
    /// Error que indica que el usuario no fue encontrado en la base de datos.
    /// </summary>
    public static readonly Error NotFound =
        new("USER_NOT_FOUND", "User not found", 404);

    /// <summary>
    /// Email es obligatorio
    /// </summary>
    public static readonly Error EmailRequired =
        new("EMAIL_REQUIRED", "Email is required", 400);

    /// <summary>
    /// El email ya está en uso
    /// </summary>
    public static readonly Error EmailInUse =
        new("EMAIL_IN_USE", "Email is already registered", 409);

    /// <summary>
    /// Formato de correo inválido
    /// </summary>
    public static readonly Error InvalidEmailFormat =
        new("INVALID_EMAIL_FORMAT", "The email format is invalid", 400);

    /// <summary>
    /// Password es obligatorio
    /// </summary>
    public static readonly Error PasswordRequired =
        new("PASSWORD_REQUIRED", "Password is required", 400);


    /// <summary>
    /// La contraseña no cumple con los requisitos mínimos de seguridad.
    /// </summary>
    public static readonly Error WeakPassword =
        new("WEAK_PASSWORD",
            "Password must be at least 8 characters long and include an uppercase letter, lowercase letter, number, and special character.", 400);
}
