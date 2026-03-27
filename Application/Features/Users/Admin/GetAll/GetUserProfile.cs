using Application.Features.Users.Resolvers.EmailResolver;
using AutoMapper;
using Domain.Entities.Users.Models;

namespace Application.Features.Users.Admin.GetAll;

public class GetUserProfile : Profile
{
    public GetUserProfile()
    {
        CreateMap<User, GetUserResponse>()
        .ForMember(dest => dest.Email,
            opt => opt.MapFrom<DecryptEmailResolver<GetUserResponse>>());
    }
}
