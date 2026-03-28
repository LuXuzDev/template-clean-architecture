namespace Domain.Shared.Abstractions;

public interface IUnitOfWork
{
    /// <summary>
    /// Guarda todos los cambios rastreados en el DbContext dentro de una transacción.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
