using System.Security.Cryptography;
using System.Text;


namespace Application.Helpers.Hasher;

public static class HasherHelper
{
    public static string Hash(string email)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(email.ToLowerInvariant());
        var hash = sha.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }


    public static string ComputeRefreshTokenHash(string refreshToken)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
        return Convert.ToBase64String(hashBytes);
    }
}
