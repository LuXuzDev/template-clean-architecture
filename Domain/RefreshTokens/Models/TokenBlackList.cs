using Domain.Shared.Abstractions;

namespace Domain.RefreshTokens.Models;

public class TokenBlackList : BaseEntity
{
    /// <summary>
    /// El access token que fue revocado
    /// </summary>
    public string Token { get; set; } = default!;

    /// <summary>
    /// Usuario propietario del token
    /// </summary>
    public string UserId { get; set; } = default!;
}
