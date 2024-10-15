using TextEncryptionApp.Models;

namespace TextEncryptionApp.EncryptionMethods
{
    public class CaesarCipher : IEncryptionMethod
    {
        private readonly int shift = 3; // Смещение по умолчанию

        public string Name => "Caesar Cipher";

        public string Encrypt(string plainText)
        {
            return new string(plainText.Select(c => (char)(c + shift)).ToArray());
        }

        public string Decrypt(string cipherText)
        {
            return new string(cipherText.Select(c => (char)(c - shift)).ToArray());
        }
    }
}
