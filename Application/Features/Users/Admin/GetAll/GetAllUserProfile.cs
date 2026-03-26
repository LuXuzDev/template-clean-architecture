using Application.Features.Users.Resolvers.EmailResolver;
using AutoMapper;
using Domain.Entities.Users.Models;

namespace Application.Features.Users.Admin.GetAll;

public class GetAllUserProfile : Profile
{
    public GetAllUserProfile()
    {
        CreateMap<User, GetAllUserResponse>()
        .ForMember(dest => dest.Email,
            opt => opt.MapFrom<DecryptEmailResolver<GetAllUserResponse>>());
    }
}
