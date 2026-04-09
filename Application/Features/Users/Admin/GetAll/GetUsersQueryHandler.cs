using Application.Services.UserValidator;
using Domain.Entities.Roles.Constants;
using Domain.Entities.Users.Repository;
using Domain.Specifications.Users;
using FastEndpoints;
using Loop.PersonalLogger;
using Shared.Results;
using IMapper = AutoMapper.IMapper;

namespace Application.Features.Users.Admin.GetAll;

public class GetUsersQueryHandler : CommandHandler<GetUsersQuery , Result<ResponseListBase<GetUserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUserValidatorService _userValidatorService;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler
        (IUserRepository userRepository,
        IUserValidatorService userValidatorService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userValidatorService = userValidatorService;
        _mapper = mapper;
    }

    public override async Task<Result<ResponseListBase<GetUserResponse>>> ExecuteAsync(GetUsersQuery query, CancellationToken ct = default)
    {
        var userClaimsResult = await _userValidatorService.ValidateAsync(ct, [RoleConstants.Admin]);

        if(userClaimsResult.IsFailure)
            return Result<ResponseListBase<GetUserResponse>>.Failure(userClaimsResult.Error!);

        var req = query.Request;
        var spec = new UsersPagedSpecification(req.PageNumber, req.PageSize, req.SortBy, req.Descending);

        var userTotalCount = await _userRepository.TotalCount(ct);
        var usersEntities = await _userRepository.ListAsync(ct, spec);

        var usersResponse = _mapper.Map<List<GetUserResponse>>(usersEntities);


        var response = ResponseListBase<GetUserResponse>.Create(
            usersResponse,
            userTotalCount,
            req.PageNumber,
            req.PageSize
        );

        return Result<ResponseListBase<GetUserResponse>>.Success(response);
    }
}
