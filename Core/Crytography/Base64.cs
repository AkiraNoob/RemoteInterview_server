using System.Text;

namespace Core.Cryptography;

public static class Base64
{
    public static string Encrypt(string? plainText)
    {
        if (plainText is null)
        {
            throw new ArgumentNullException(nameof(plainText));
        }

        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    public static string Decrypt(string cipherText)
    {
        if (cipherText is null)
        {
            throw new ArgumentNullException(nameof(cipherText));
        }

        return Encoding.UTF8.GetString(Convert.FromBase64String(cipherText));
    }
}