using Domain.RefreshTokens.Models;

namespace Domain.RefreshTokens.Repository;

public interface IRefreshTokenRepository
{
    Task CreateAsync(RefreshToken refreshToken);
}
