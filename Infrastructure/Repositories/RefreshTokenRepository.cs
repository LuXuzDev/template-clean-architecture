using Domain.Entities.RefreshTokens.Models;
using Domain.Entities.RefreshTokens.Repository;


namespace Infrastructure.Repositories;

public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
