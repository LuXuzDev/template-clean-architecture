using Domain.Users.Models;

namespace Domain.Users.Repository;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<User?> GetByEmailAsync(string hashedEmail, CancellationToken ct);

    Task CreateAsync (User user);
}
