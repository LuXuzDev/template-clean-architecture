using Domain.Entities.Users.Models;

namespace Domain.Specifications.Users;

public sealed class UserByEmailSpecification : Specification<User>
{
    public UserByEmailSpecification(string hashedEmail)
    {
        Criteria = u => u.HashedEmail == hashedEmail;

        AddInclude(u => u.Role!);
        //Para hacen ThenInclude
        //AddInclude(u => u.Roles.Select(r => r.Permissions));
    }
}