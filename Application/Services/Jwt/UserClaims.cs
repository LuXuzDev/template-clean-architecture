using Domain.Entities.Roles.Constants;

namespace Application.Services.Jwt;

public class UserClaims
{
    public Guid UserId { get; set; }
    public string? Role { get; set; }
    public bool HasRole(string role) => Role?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
    public bool IsAdmin => HasRole(RoleConstants.Admin);
    public bool IsClient => HasRole(RoleConstants.Client);
}