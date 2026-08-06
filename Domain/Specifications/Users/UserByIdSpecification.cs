using Domain.Entities.Users.Models;

namespace Domain.Specifications.Users;

public sealed class UserByIdSpecification : Specification<User>
{
    public UserByIdSpecification(Guid userId)
    {
        Criteria = u => u.Id == userId;

        AddInclude(u => u.Role!);
        //Para hacen ThenInclude
        /*
        AddInclude(q => q
        .Include(b => b.DishGroups)
            .ThenInclude(dg => dg.Dishes)
                .ThenInclude(d => d.DishImages));
        */
    }

    public UserByIdSpecification(string userIdString) :
        this(Guid.Parse(userIdString)) { }
}