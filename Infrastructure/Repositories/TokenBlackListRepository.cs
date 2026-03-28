using Domain.Entities.RefreshTokens.Models;
using Domain.Entities.RefreshTokens.Repository;


namespace Infrastructure.Repositories;

public class TokenBlackListRepository : Repository<TokenBlackList>, ITokenBlackListRepository
{
    public TokenBlackListRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
