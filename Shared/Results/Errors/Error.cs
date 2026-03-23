namespace Shared.Results.Errors;

/// <summary>
/// Representa un error que puede ocurrir durante la ejecución de una operación.
/// </summary>
/// <remarks>
/// Este tipo se utiliza junto con <see cref="Result"/> o <see cref="Result{T}"/> para 
/// devolver información estructurada de errores en la capa de aplicación o API.
/// Incluye un código único, un mensaje descriptivo y el código HTTP asociado.
/// </remarks>
/// <param name="Code">Código único del error, usado para identificarlo de forma estable.</param>
/// <param name="Message">Mensaje descriptivo del error, útil para mostrar al usuario o logs.</param>
/// <param name="HttpCode">Código HTTP sugerido para devolver en la API.</param>
public record Error(string Code, string Message, int HttpCode);