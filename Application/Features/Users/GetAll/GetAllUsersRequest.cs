using Domain.Specifications.Sorts;
using Shared.Requests;

namespace Application.Features.Users.GetAll;

public class GetAllUsersRequest : RequestListBase
{
    public required UserSort SortBy { get; set; }
    public required bool Descending { get; set; }
}
