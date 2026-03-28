using Domain.Specifications;

namespace Domain.Shared.Abstractions;

public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Obtiene una lista de entidades. Si se proporciona una especificación, aplica los filtros.
    /// </summary>
    Task<List<T>> ListAsync(CancellationToken ct, Specification<T>? spec = null);

    /// <summary>
    /// Obtiene la primera entidad que coincide con la especificación, o null si no existe.
    /// </summary>
    Task<T?> FirstOrDefaultAsync(Specification<T> spec, CancellationToken ct);

    /// <summary>
    /// Verifica si existe al menos una entidad que coincide con la especificación.
    /// </summary>
    Task<bool> AnyAsync(Specification<T> spec, CancellationToken ct);

    /// <summary>
    /// Agrega una nueva entidad al contexto (sin persistir). Requiere UnitOfWork.SaveChangesAsync.
    /// </summary>
    Task CreateAsync(T entity);

    /// <summary>
    /// Marca una entidad existente como modificada (sin persistir). Requiere UnitOfWork.SaveChangesAsync.
    /// </summary>
    Task UpdateAsync(T entity, CancellationToken ct);

    /// <summary>
    /// Marca una entidad para eliminación lógica o física (sin persistir). Requiere UnitOfWork.SaveChangesAsync.
    /// </summary>
    Task DeleteAsync(T entity, CancellationToken ct);

    /// <summary>
    /// Marca una entidad como eliminada (IsDeleted = true) sin persistir. Requiere UnitOfWork.SaveChangesAsync.
    /// </summary>
    Task SoftDeleteAsync(T entity, CancellationToken ct);

    /// <summary>
    /// Marca múltiples entidades para eliminación (sin persistir). Requiere UnitOfWork.SaveChangesAsync.
    /// </summary>
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken ct);

    /// <summary>
    /// Marca múltiples entidades como eliminadas (IsDeleted = true) sin persistir. Requiere UnitOfWork.SaveChangesAsync.
    /// </summary>
    Task SoftDeleteRangeAsync(IEnumerable<T> entities, CancellationToken ct);

    /// <summary>
    /// Elimina múltiples entidades que coinciden con la especificación. Retorna el conteo afectado.
    /// </summary>
    Task<int> DeleteBySpecAsync(Specification<T> spec, CancellationToken ct);

    /// <summary>
    /// Marca como eliminadas las entidades que coinciden con la especificación. Retorna el conteo afectado.
    /// </summary>
    Task<int> SoftDeleteBySpecAsync(Specification<T> spec, CancellationToken ct);

    /// <summary>
    /// Obtiene el total de entidades. Si se proporciona una especificación, aplica los filtros.
    /// </summary>
    Task<int> TotalCount(CancellationToken ct, Specification<T>? spec = null);
}