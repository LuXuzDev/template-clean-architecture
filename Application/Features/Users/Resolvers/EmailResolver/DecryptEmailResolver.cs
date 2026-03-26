using Application.Services.DataProtector;
using AutoMapper;
using Domain.Entities.Users.Models;


namespace Application.Features.Users.Resolvers.EmailResolver;

public class DecryptEmailResolver<TDestination> : IValueResolver<User, TDestination, string>
{
    private readonly IDataProtectorFactory _dataProtectorFactory;

    public DecryptEmailResolver(IDataProtectorFactory dataProtectorFactory)
    {
        _dataProtectorFactory = dataProtectorFactory;
    }

    public string Resolve(User source, TDestination destination, string destMember, ResolutionContext context)
    {
        var emailProtector = _dataProtectorFactory.Create(DataPorpuse.UserEmail);

        return emailProtector.Unprotect(source.EncryptedEmail);
    }
}