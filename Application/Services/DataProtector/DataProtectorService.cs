using Microsoft.AspNetCore.DataProtection;


namespace Application.Services.DataProtector;

public class DataProtectorService : IDataProtectorService
{
    private readonly IDataProtector _protector;

    // El protector se crea con un propósito único (ej: "User.Email", "User.ResetPassword")
    public DataProtectorService(IDataProtector protector)
    {
        _protector = protector;
    }

    // Cifrar
    public string Protect(string data)
        => _protector.Protect(data);

    // Descifrar
    public string Unprotect(string cipher)
        => _protector.Unprotect(cipher);
}
