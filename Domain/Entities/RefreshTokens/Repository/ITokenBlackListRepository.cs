using Domain.Entities.RefreshTokens.Models;
using Domain.Shared.Abstractions;

namespace Domain.Entities.RefreshTokens.Repository;

public interface ITokenBlackListRepository : IRepository<TokenBlackList>
{
}
