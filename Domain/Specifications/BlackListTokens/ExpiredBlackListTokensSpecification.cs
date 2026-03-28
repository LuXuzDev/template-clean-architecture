using Domain.Entities.RefreshTokens.Models;

namespace Domain.Specifications.BlackListTokens;


/// <summary>
/// Especificación para obtener tokens de la blacklist más antiguos que el tiempo de expiración dado.
/// </summary>
public class ExpiredBlackListTokensSpecification : Specification<TokenBlackList>
{
    public ExpiredBlackListTokensSpecification(int expirationInMinutes, DateTime? referenceDate = null)
    {
        var now = referenceDate ?? DateTime.UtcNow;
        var cutoffTime = now.AddMinutes(-expirationInMinutes);

        Criteria = t => t.CreatedAt <= cutoffTime;
    }
}