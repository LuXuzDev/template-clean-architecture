using Domain.Entities.Roles.Models;
using Domain.Entities.Roles.Repository;


namespace Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
