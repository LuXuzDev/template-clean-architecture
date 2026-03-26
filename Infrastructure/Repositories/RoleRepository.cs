using Domain.Entities.Roles.Models;
using Domain.Entities.Roles.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _dbContext;

    public RoleRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken ct)
    {
        return await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name, ct);
    }
}
