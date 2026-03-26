using Domain.Entities.RefreshTokens.Models;
using Domain.Entities.RefreshTokens.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _dbContext;

    public RefreshTokenRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow;
        var query = _dbContext.RefreshTokens.Where(x => x.TokenHash == refreshToken && x.RevokedAt == null);
        
        query = query.Where(i => !i.IsDeleted);

        return await query.OrderByDescending(ord => ord.CreatedAt).FirstOrDefaultAsync(ct);
    }

    public async Task<int> DeleteExpiredOrRevokedAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        return await _dbContext.RefreshTokens
            .Where(t => t.RevokedAt != null || t.ExpiresAt <= now)
            .ExecuteDeleteAsync(ct);
    }
    public async Task UpdateAsync(RefreshToken refreshToken)
    {
        refreshToken.UpdatedAt = DateTime.UtcNow;
        _dbContext.RefreshTokens.Update(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(string hashedRefreshToken, CancellationToken ct = default)
    {
        var token = await _dbContext.RefreshTokens
            .FirstOrDefaultAsync(t => t.TokenHash == hashedRefreshToken, ct);

        if (token != null)
        {
            _dbContext.RefreshTokens.Remove(token);
            await _dbContext.SaveChangesAsync(ct);
        }
    }
}
