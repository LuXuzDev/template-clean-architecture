using Microsoft.AspNetCore.DataProtection;


namespace Application.Services.DataProtector;


public class DataProtectorFactory : IDataProtectorFactory
{
    private readonly IDataProtectionProvider _provider;

    public DataProtectorFactory(IDataProtectionProvider provider)
    {
        _provider = provider;
    }

    public IDataProtectorService Create(string purpose)
    {
        var protector = _provider.CreateProtector(purpose);
        return new DataProtectorService(protector);
    }
}