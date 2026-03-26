using Domain.Entities.Users.Models;

namespace Domain.Specifications.Users;

public sealed class UserByIdSpecification : Specification<User>
{
    public UserByIdSpecification(string idString)
    {
        var id = Guid.Parse(idString);
        Criteria = u => u.Id == id;

        AddInclude(u => u.Role!);
        //Para hacen ThenInclude
        //AddInclude(u => u.Roles.Select(r => r.Permissions));
    }

    public UserByIdSpecification(Guid id)
    {
        Criteria = u => u.Id == id;

        AddInclude(u => u.Role!);
        //Para hacen ThenInclude
        //AddInclude(u => u.Roles.Select(r => r.Permissions));
    }
}