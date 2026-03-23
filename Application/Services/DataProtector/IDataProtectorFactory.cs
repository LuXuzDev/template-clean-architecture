namespace Application.Services.DataProtector;

public interface IDataProtectorFactory
{
    IDataProtectorService Create(string purpose);
}
