using Defender.Common.Enums;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Defender.Common.Helpers;

public class CryptographyHelper
{
    public static async Task<string> EncryptStringAsync(string plainText, string salt = "")
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = HexStringToByteArray(await SecretsHelper.GetSecretAsync(Secret.SecretsEncryptionKey));
            aesAlg.IV = GenerateIV(Encoding.UTF8.GetBytes(salt));

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }
                return Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
    }

    public static async Task<string> DecryptStringAsync(string cipherText, string salt = "")
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = HexStringToByteArray(await SecretsHelper.GetSecretAsync(Secret.SecretsEncryptionKey));
            aesAlg.IV = GenerateIV(Encoding.UTF8.GetBytes(salt));

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    static byte[] GenerateIV(byte[] salt)
    {
        byte[] keyBytes = new byte[16];

        for (int i = 0; i < keyBytes.Length; i++)
        {
            keyBytes[i] = (byte)(salt[i % salt.Length]);
        }

        return keyBytes;
    }

    static byte[] HexStringToByteArray(string hex)
    {
        int numberChars = hex.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }
}
