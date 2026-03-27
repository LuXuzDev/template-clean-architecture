using Domain.Specifications.Sorts;
using Shared.Requests;

namespace Application.Features.Users.Admin.GetAll;

public class GetUsersRequest : RequestListBase
{
    public required UserSort SortBy { get; set; }
    public required bool Descending { get; set; }
}
