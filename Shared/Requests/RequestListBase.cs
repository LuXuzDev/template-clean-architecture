namespace Shared.Requests;

/// <summary>
/// Clase base para solicitudes que requieren paginación.
/// </summary>
/// <remarks>
/// Proporciona los parámetros comunes utilizados en operaciones de consulta que retornan listas paginadas.
/// Puede ser heredada por cualquier request que necesite incluir filtros, búsqueda o paginación.
/// </remarks>
public abstract class RequestListBase
{
    /// <summary>
    /// Número de página solicitada (comienza en 1).
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Cantidad de elementos por página.
    /// </summary>
    public int PageSize { get; set; } = 12;
}
