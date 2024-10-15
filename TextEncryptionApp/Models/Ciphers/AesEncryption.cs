using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TextEncryptionApp.Models;

namespace TextEncryptionApp.EncryptionMethods
{
    public class AesEncryption : IEncryptionMethod
    {
        private readonly byte[] keyBytes; // Ваш AES ключ (32 байта)
        private readonly byte[] iv; // IV (16 байт)

        public AesEncryption(string key = "13f73f6d0d3790e5c79412b3222e06d0202d6f9b933e7359a5eb847ed73c52d9") // Ключ по умолчанию
        {
            // Убедитесь, что ключ имеет правильный размер (256 бит)
            if (key.Length != 64) // 32 байта в hex формате
            {
                throw new ArgumentException("Key must be 64 characters long (32 bytes in hex).");
            }

            keyBytes = ConvertHexStringToByteArray(key); // Преобразуем hex строку в массив байтов
            iv = new byte[16]; // IV должен быть 16 байт (128 бит)
        }

        public string Name => "AES Encryption";

        public string Encrypt(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();
                aesAlg.Mode = CipherMode.CBC;

                // Запоминаем IV для использования в расшифровке
                byte[] iv = aesAlg.IV;

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv))
                {
                    byte[] encrypted = PerformCryptography(Encoding.UTF8.GetBytes(plainText), encryptor);
                    // Сохраняем IV перед зашифрованными данными
                    byte[] combined = new byte[iv.Length + encrypted.Length];
                    Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
                    Buffer.BlockCopy(encrypted, 0, combined, iv.Length, encrypted.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                byte[] combined = Convert.FromBase64String(cipherText);

                // Извлекаем IV из начала зашифрованного текста
                byte[] iv = new byte[16]; // IV должен быть 16 байт
                Buffer.BlockCopy(combined, 0, iv, 0, iv.Length);

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv))
                {
                    // Извлекаем зашифрованные данные (без IV)
                    byte[] encryptedData = new byte[combined.Length - iv.Length];
                    Buffer.BlockCopy(combined, iv.Length, encryptedData, 0, encryptedData.Length);

                    byte[] decrypted = PerformCryptography(encryptedData, decryptor);
                    return Encoding.UTF8.GetString(decrypted);
                }
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        private byte[] ConvertHexStringToByteArray(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}
