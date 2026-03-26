namespace Shared.Results.Errors.Pagination;

public static class PaginationErrors
{
    /// <summary>
    /// Error que indica que el número de página debe ser mayor o igual a 1.
    /// </summary>
    public static readonly Error InvalidPageNumber =
        new("PAGINATION_INVALID_PAGE_NUMBER", "Page number must be greater than or equal to 1.", 400);

    /// <summary>
    /// Error que indica que el tamaño de página debe ser mayor o igual a 1.
    /// </summary>
    public static readonly Error PageSizeTooSmall =
        new("PAGINATION_PAGE_SIZE_TOO_SMALL", "Page size must be greater than or equal to 1.", 400);

    /// <summary>
    /// Error que indica que el tamaño de página no puede ser mayor a 100.
    /// </summary>
    public static readonly Error PageSizeTooLarge =
        new("PAGINATION_PAGE_SIZE_TOO_LARGE", "Page size cannot be greater than 100.", 400);

    /// <summary>
    /// Valor de ordenamiento inválido
    /// </summary>
    public static readonly Error InvalidSortBy =
        new("INVALID_SORT_BY", "Invalid SortBy value", 400);
}