using Domain.Entities.RefreshTokens.Models;
using Domain.Entities.RefreshTokens.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TokenBlackListRepository : ITokenBlackListRepository
{
    private readonly AppDbContext _dbContext;

    public TokenBlackListRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(TokenBlackList tokenBlackList, CancellationToken ct = default)
    {
        await _dbContext.TokenBlackList.AddAsync(tokenBlackList, ct);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteExpiredAsync(int expirationInMinutes, CancellationToken ct = default)
    {
        var cutoffTime = DateTime.UtcNow.AddMinutes(-expirationInMinutes);

        var deletedCount = await _dbContext.TokenBlackList
            .Where(t => t.CreatedAt <= cutoffTime)
            .ExecuteDeleteAsync(ct);

        return deletedCount;
    }

    public async Task<bool> ExistsAsync(string token, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        return await _dbContext.TokenBlackList
            .AnyAsync(t => t.Token == token, ct);
    }
}
