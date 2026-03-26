using Domain.Entities.Roles.Models;

namespace Domain.Entities.Roles.Repository;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken ct);
}
