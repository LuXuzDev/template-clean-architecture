using Domain.Entities.RefreshTokens.Models;


namespace Domain.Specifications.RefreshTokens;

public class RefreshTokenByTokenSpecification : Specification<RefreshToken>
{
    public RefreshTokenByTokenSpecification(string hashedToken)
    {
        Criteria = rt => rt.TokenHash == hashedToken;
    }
}