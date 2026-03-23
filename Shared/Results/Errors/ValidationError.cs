namespace Shared.Results.Errors;

/// <summary>
/// Representa un error de validación de una propiedad o entidad.
/// </summary>
/// <param name="Code">Código único del error de validación.</param>
/// <param name="Message">Mensaje descriptivo del error.</param>
/// <param name="PropertyName">Nombre de la propiedad o campo que falló la validación.</param>
public record ValidationError(string Code, string Message, string? PropertyName = null) : Error(Code, Message,400);