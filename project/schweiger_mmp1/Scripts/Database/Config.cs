using System.Security.Cryptography;
using System.Text;

public static class Config
{
    private const string EncryptionKey_Leaderboard = "lilguy"; 
    private const string EncryptionKey_Imposter = "imposter"; 

    private const string EncryptedConnString_Leaderboard = "3atuG/bnfSzGO44+zvsS6oy7+H4I0TWfyIqminYWY43pTQVuz9bVl4ue1zUOw4q6uIE8PCJMt0s2saxdf83ohYi+SIqiiL/2B4Bbq8Di04eny5U97ZgDio1EyLHkuvgbRbQIvl5flwD4HdRp55iWzg==";
    private const string EncryptedConnString_Imposter = "2y7QifGUQyzfIgFiVvTzsihaAJCWDk5ZVRlZhIRGx1M/ZwgtZ59TV6t/7ZolcTJjeL7k7oeGirTNHAl1L+vtP0atqIEld2yzAQim6gDS01aB/17ecPdwYiVmysXyhT/1+2bWZgPLdMzEp7sqojir5g==";

    public static string connString_Leaderboard => Decrypt(EncryptedConnString_Leaderboard, EncryptionKey_Leaderboard);
    public static string connString_Imposter => Decrypt(EncryptedConnString_Imposter, EncryptionKey_Imposter);

    private static byte[] GetKey(string key)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
    }

    private static string Decrypt(string encryptedText, string key)
    {
        byte[] keyBytes = GetKey(key);
        byte[] buffer = Convert.FromBase64String(encryptedText);
        using (Aes aes = Aes.Create())
        {
            byte[] iv = new byte[aes.BlockSize / 8];
            byte[] ciphertext = new byte[buffer.Length - iv.Length];
            Array.Copy(buffer, iv, iv.Length);
            Array.Copy(buffer, iv.Length, ciphertext, 0, ciphertext.Length);
            aes.Key = keyBytes;
            aes.IV = iv;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(ciphertext, 0, ciphertext.Length);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
