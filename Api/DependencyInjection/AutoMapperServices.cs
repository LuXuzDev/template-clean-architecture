using Application.Features.Users.GetAll;
using Application.Features.Users.Resolvers.EmailResolver;



namespace Api.DependencyInjection;

public static class AutoMapperServices
{
    public static IServiceCollection AddAutoMapperServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region User

        //Resolvers
        services.AddScoped(typeof(DecryptEmailResolver<>));


        //Mappers
        services.AddAutoMapper(cfg => cfg.AddProfile<GetAllUserProfile>());

        #endregion

        return services;
    }
}
