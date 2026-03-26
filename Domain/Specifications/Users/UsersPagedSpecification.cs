using Domain.Entities.Users.Models;
using Domain.Specifications.Sorts;


namespace Domain.Specifications.Users;

public sealed class UsersPagedSpecification : Specification<User>
{
    public UsersPagedSpecification(
        int page,
        int pageSize,
        UserSort sort,
        bool descending)
    {
        ApplyPaging((page - 1) * pageSize, pageSize);

        ApplySorting(sort, descending);
    }


    private void ApplySorting(UserSort sort, bool descending)
    {
        switch (sort)
        {
            case UserSort.CreatedAt:
                if (descending)
                    ApplyOrderByDescending(u => u.CreatedAt);
                else
                    ApplyOrderBy(u => u.CreatedAt);
                break;


            default:
                ApplyOrderByDescending(u => u.CreatedAt);
                break;
        }
    }
}