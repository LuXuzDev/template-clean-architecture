namespace Shared.Responses;


/// <summary>
/// Clase base genérica que representa una respuesta paginada de una lista de elementos.
/// </summary>
/// <typeparam name="T">Tipo de los elementos incluidos en la lista.</typeparam>
public class ResponseListBase<T>
{
    /// <summary>
    /// Elementos correspondientes a la página actual.
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// Número total de registros que cumplen con los criterios de búsqueda.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Número de página actual (comienza en 1).
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Cantidad de elementos por página.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Número total de páginas disponibles según <see cref="TotalCount"/> y <see cref="PageSize"/>.
    /// </summary>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indica si existe una página anterior.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indica si existe una página siguiente.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;
}
