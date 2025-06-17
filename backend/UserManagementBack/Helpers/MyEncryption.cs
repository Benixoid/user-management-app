using System.Globalization;
using System.Security.Cryptography;

namespace UserManagementBack.Helpers
{
    internal static class MyEncryption
    {
        private static readonly string security_key = "4D-8B-BF-AB-F8-09-97-CB-D4-CE-ED-E6-F3-A7-5E-25-26-A1-0A-3A-18-FD-9E-2C-37-CD-AB-DC-70-9B-52-D2";
        private static readonly string security_vector = "2B-6D-74-82-D1-D3-D8-90-13-05-9F-95-7B-9C-E7-6F";
        private static byte[] MyEncryptionHelper(this string val)
        {
            List<string> str_list = val.Split('-').ToList();
            List<byte> bytes_list = new();
            foreach (string item in str_list)
            {
                bytes_list.Add(byte.Parse(item, NumberStyles.HexNumber));
            }
            return bytes_list.ToArray();
        }
        private static string MyEncryptionHelper(this byte[] val)
        {
            return BitConverter.ToString(val);
        }
        private static void KeyGeneration()
        {
            //Генерация ключей
            Aes crypt = Aes.Create();
            crypt.GenerateKey();
            string key = BitConverter.ToString(crypt.Key);
            string vector = BitConverter.ToString(crypt.IV);
        }

        public static string EncryptString(string plainText)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException(nameof(plainText));
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = security_key.MyEncryptionHelper();
                aesAlg.IV = security_vector.MyEncryptionHelper();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using MemoryStream msEncrypt = new();
                using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
                        
            return encrypted.MyEncryptionHelper();
        }

        public static string DecryptString(string encrText)
        {
            string plaintext = "";
            try
            {                
                if (encrText == null || encrText.Length <= 0)
                    throw new ArgumentNullException(nameof(encrText));

                byte[] cipherText = encrText.MyEncryptionHelper();

                using Aes aesAlg = Aes.Create();
                aesAlg.Key = security_key.MyEncryptionHelper();
                aesAlg.IV = security_vector.MyEncryptionHelper();

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using MemoryStream msDecrypt = new(cipherText);
                using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new(csDecrypt);

                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plaintext = srDecrypt.ReadToEnd();
            }
            catch (Exception)
            {

            }
            return plaintext;
        }
    }
}
