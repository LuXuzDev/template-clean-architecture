using Domain.Shared.Abstractions;

namespace Domain.Roles.Models;

public class Role : BaseEntity
{
    public required string Name { get; set; }
}
