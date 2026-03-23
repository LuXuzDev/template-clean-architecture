using System.Security.Cryptography;
using System.Text;


namespace Application.Helpers;

public static class HasherHelper
{
    public static string Hash(string email)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(email.ToLowerInvariant());
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
