using Domain.Roles.Models;

namespace Domain.Roles.Repository;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name, CancellationToken ct);
}
