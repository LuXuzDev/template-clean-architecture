using FastEndpoints;
using Shared.Results;

namespace Application.Features.Users.Admin.GetAll;

public class GetUsersQuery : ICommand<Result<ResponseListBase<GetUserResponse>>>
{
    public GetUsersRequest Request { get; set; } = null!;
}
