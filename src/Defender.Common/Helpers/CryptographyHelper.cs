using Defender.Common.Enums;
using System.Security.Cryptography;
using System.Text;

namespace Defender.Common.Helpers;

public class CryptographyHelper
{
    public static async Task<string> EncryptString(string plainText, string salt = "")
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(await SecretsHelper.GetSecretAsync(Secret.SecretsEncryptionKey));
            aesAlg.IV = Encoding.UTF8.GetBytes(salt);

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

    public static async Task<string> DecryptString(string cipherText, string salt = "")
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(await SecretsHelper.GetSecretAsync(Secret.SecretsEncryptionKey));
            aesAlg.IV = Encoding.UTF8.GetBytes(salt);

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
}
