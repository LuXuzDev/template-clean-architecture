using Domain.Entities.RefreshTokens.Models;

namespace Domain.Specifications.BlackListTokens;

public class BlackListTokenByTokenSpecification : Specification<TokenBlackList>
{
    public BlackListTokenByTokenSpecification(string token)
    {
        Criteria = rt => rt.Token == token;
    }
}