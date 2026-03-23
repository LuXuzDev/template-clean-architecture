using Shared.Requests;
using Shared.Responses;

namespace Application.Helpers;


/// <summary>
/// Proporciona métodos utilitarios para aplicar paginación sobre colecciones en memoria.
/// Diseñado para usarse en la capa de aplicación (después del mapeo de entidades a DTOs).
/// </summary>
public static class PaginationHelper
{
    /// <summary>
    /// Aplica paginación sobre una lista en memoria y devuelve una respuesta paginada.
    /// </summary>
    /// <typeparam name="T">Tipo de elemento contenido en la lista.</typeparam>
    /// <param name="source">Colección ya materializada (por ejemplo, una lista de DTOs o responses).</param>
    /// <param name="request">Parámetros de paginación y ordenamiento.</param>
    /// <returns>
    /// Una instancia de <see cref="ResponseListBase{T}"/> que contiene los elementos paginados 
    /// y metadatos como total de registros, página actual y total de páginas.
    /// </returns>
    public static ResponseListBase<T> Paginate<T>(
        IEnumerable<T> source,
        RequestListBase request)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        // Total de registros
        var totalCount = source.Count();

        // Aplicar paginación en memoria
        var items = source
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Construir respuesta
        return new ResponseListBase<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}
