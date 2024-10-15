using System;
using System.Linq;
using TextEncryptionApp.Models;

namespace TextEncryptionApp.EncryptionMethods
{
    public class VigenereCipher : IEncryptionMethod
    {
        private readonly string keyword;

        public VigenereCipher(string keyword)
        {
            this.keyword = keyword.ToUpper(); // Преобразуем ключевое слово в верхний регистр для удобства
        }

        public string Name => "Vigenère Cipher";

        private char EncryptChar(char plainChar, char keyChar)
        {
            if (!char.IsLetter(plainChar)) return plainChar;

            char offset = char.IsUpper(plainChar) ? 'A' : 'a';
            return (char)((plainChar + keyChar - 2 * offset) % 26 + offset);
        }

        private char DecryptChar(char cipherChar, char keyChar)
        {
            if (!char.IsLetter(cipherChar)) return cipherChar;

            char offset = char.IsUpper(cipherChar) ? 'A' : 'a';
            return (char)((cipherChar - keyChar + 26) % 26 + offset);
        }

        public string Encrypt(string plainText)
        {
            plainText = plainText.ToUpper(); // Преобразуем текст в верхний регистр для удобства
            return string.Concat(plainText.Select((c, i) => EncryptChar(c, keyword[i % keyword.Length])));
        }

        public string Decrypt(string cipherText)
        {
            cipherText = cipherText.ToUpper(); // Преобразуем текст в верхний регистр для удобства
            return string.Concat(cipherText.Select((c, i) => DecryptChar(c, keyword[i % keyword.Length])));
        }
    }
}
