using Domain.Specifications;

namespace Domain.Shared.Abstractions;

public interface IRepository<T> where T : BaseEntity
{
    Task<List<T>> ListAsync(CancellationToken ct, Specification<T>? spec = null);
    Task<T?> FirstOrDefaultAsync(Specification<T> spec, CancellationToken ct);
    Task<bool> AnyAsync(Specification<T> spec, CancellationToken ct);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity, CancellationToken ct);
    Task SoftDeleteAsync(T entity, CancellationToken ct);
    Task DeleteAsync(T entity, CancellationToken ct);
    Task<int> TotalCount(CancellationToken ct, Specification<T>? spec = null);
}