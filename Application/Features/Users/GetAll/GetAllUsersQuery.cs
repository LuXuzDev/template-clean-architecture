using FastEndpoints;
using Shared.Results;

namespace Application.Features.Users.GetAll;

public class GetAllUsersQuery : ICommand<Result<ResponseListBase<GetAllUserResponse>>>
{
    public GetAllUsersRequest Request { get; set; } = null!;
}
