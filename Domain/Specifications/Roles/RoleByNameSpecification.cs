using Domain.Entities.Roles.Models;


namespace Domain.Specifications.Roles;

public class RoleByNameSpecification : Specification<Role>
{
    public RoleByNameSpecification(string name)
    {
        Criteria = r => r.Name == name;
    }
}