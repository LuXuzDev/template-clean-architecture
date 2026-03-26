using Domain.Shared.Abstractions;

namespace Domain.Entities.Roles.Models;

public class Role : BaseEntity
{
    public required string Name { get; set; }
}
