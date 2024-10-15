using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TextEncryptionApp.Models;

namespace TextEncryptionApp.EncryptionMethods
{
    public class TripleDESAlgorithm : IEncryptionMethod
    {
        private readonly string key; // Ключ для шифрования

        public TripleDESAlgorithm(string key)
        {
            this.key = key;
        }

        public string Name => "Triple DES";

        // Метод для шифрования
        public string Encrypt(string plainText)
        {
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = GenerateKeyFromString(key);
                tdes.Mode = CipherMode.ECB; // Режим шифрования (можно поменять на CBC для большей безопасности)
                tdes.Padding = PaddingMode.PKCS7;

                byte[] data = Encoding.UTF8.GetBytes(plainText);
                ICryptoTransform transform = tdes.CreateEncryptor();

                byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(result);
            }
        }

        // Метод для дешифрования
        public string Decrypt(string encryptedText)
        {
            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                tdes.Key = GenerateKeyFromString(key);
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                byte[] data = Convert.FromBase64String(encryptedText);
                ICryptoTransform transform = tdes.CreateDecryptor();

                byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                return Encoding.UTF8.GetString(result);
            }
        }

        // Генерация ключа из строки, так как 3DES требует 24 байта
        private byte[] GenerateKeyFromString(string key)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
                byte[] truncatedHash = new byte[24];
                Array.Copy(hash, truncatedHash, 24);
                return truncatedHash;
            }
        }
    }
}
