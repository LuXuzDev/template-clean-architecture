namespace Shared.Results.Errors.Role;

public static class RoleErrors
{
    /// <summary>
    /// Error que indica que el rol no fue encontrado en la base de datos.
    /// </summary>
    public static readonly Error NotFound =
        new("ROL_NOT_FOUND", "Rol not found", 404);
}
