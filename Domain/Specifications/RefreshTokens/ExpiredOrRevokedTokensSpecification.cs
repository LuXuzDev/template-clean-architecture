using Domain.Entities.RefreshTokens.Models;


namespace Domain.Specifications.RefreshTokens;

public class ExpiredOrRevokedTokensSpecification : Specification<RefreshToken>
{
    public ExpiredOrRevokedTokensSpecification(DateTime? referenceDate = null)
    {
        var now = referenceDate ?? DateTime.UtcNow;

        Criteria = t => t.RevokedAt != null || t.ExpiresAt <= now;
    }
}
