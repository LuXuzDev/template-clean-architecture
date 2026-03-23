using Shared.Results.Errors;

namespace Shared.Results;


/// <summary>
/// Resultado de validación que puede contener múltiples errores.
/// </summary>
public class ValidationResult
{
    /// <summary>
    /// Indica si la validación fue exitosa.
    /// </summary>
    public bool IsValid => !Errors.Any();

    /// <summary>
    /// Lista de errores de validación.
    /// </summary>
    public List<ValidationError> Errors { get; } = new();

    /// <summary>
    /// Agrega un nuevo error de validación.
    /// </summary>
    /// <param name="error">Error a agregar.</param>
    public void AddError(ValidationError error) => Errors.Add(error);

    /// <summary>
    /// Agrega múltiples errores de validación.
    /// </summary>
    /// <param name="errors">Errores a agregar.</param>
    public void AddErrors(IEnumerable<ValidationError> errors) => Errors.AddRange(errors);
}