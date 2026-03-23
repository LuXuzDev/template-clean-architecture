namespace Application.Services.DataProtector;

public interface IDataProtectorService
{
    /// <summary>
    /// Cifra un texto (reversible)
    /// </summary>
    string Protect(string data);

    /// <summary>
    /// Descifra un texto cifrado (lanza excepción si es inválido)
    /// </summary>
    string Unprotect(string cipher);
}


