public class ResponseListBase<T>
{
    /// <summary>
    /// Elementos correspondientes a la página actual.
    /// </summary>
    public List<T> Items { get; init; }

    /// <summary>
    /// Número total de registros que cumplen con los criterios de búsqueda.
    /// </summary>
    public int TotalCount { get; init; }

    /// <summary>
    /// Número de página actual (comienza en 1).
    /// </summary>
    public int PageNumber { get; init; }

    /// <summary>
    /// Cantidad de elementos por página.
    /// </summary>
    public int PageSize { get; init; }

    /// <summary>
    /// Número total de páginas disponibles según <see cref="TotalCount"/> y <see cref="PageSize"/>.
    /// </summary>
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);

    /// <summary>
    /// Indica si existe una página anterior.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indica si existe una página siguiente.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Constructor que recibe items, página, tamaño y totalCount.
    /// </summary>
    public ResponseListBase(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items.ToList();
        TotalCount = totalCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
    }

    /// <summary>
    /// Creador estático para mayor legibilidad.
    /// </summary>
    public static ResponseListBase<T> Create(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        => new ResponseListBase<T>(items, totalCount, pageNumber, pageSize);
}