using Xunit;
using TextEncryptionApp.EncryptionMethods;

namespace TextEncryptionApp.Tests
{
    public class EncryptionMethodsTests
    {
        [Theory]
        [InlineData("HELLO", "KHOOR")] // Шифр Цезаря с сдвигом 3
        public void CaesarCipher_Encrypt_ReturnsExpected(string input, string expected)
        {
            // Arrange
            var cipher = new CaesarCipher();

            // Act
            var result = cipher.Encrypt(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("HELLO", "KEY", "RIJVS UYVJN")] // Шифр Виженера
        public void VigenereCipher_Encrypt_ReturnsExpected(string input, string key, string expected)
        {
            // Arrange
            var cipher = new VigenereCipher(key);

            // Act
            var result = cipher.Encrypt(input);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
