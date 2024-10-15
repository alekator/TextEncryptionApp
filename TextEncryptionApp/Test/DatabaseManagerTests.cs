using System;
using System.Collections.Generic;
using Xunit;
using Npgsql;
using TextEncryptionApp.Models;

namespace TextEncryptionApp.Test
{
    public class DatabaseManagerTests
    {
        private readonly DatabaseManager _dbManager;
        private const string ConnectionString = "Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase"; // Убедитесь, что строка подключения корректна.

        public DatabaseManagerTests()
        {
            _dbManager = new DatabaseManager(ConnectionString);
        }

        [Fact]
        public void SaveEncryptedText_ShouldSaveTextAndReturnId()
        {
            // Arrange
            string encryptedText = "test_encrypted_text";
            string encryptionMethod = "AES";

            // Act
            int newId = _dbManager.SaveEncryptedText(encryptedText, encryptionMethod);

            // Assert
            Assert.True(newId > 0); // Проверяем, что ID положительный (текст был сохранен)
        }

        [Fact]
        public void GetAllEncryptedTexts_ShouldReturnListOfEncryptedRecords()
        {
            // Arrange
            string encryptedText1 = "test_encrypted_text_1";
            string encryptionMethod1 = "AES";
            string encryptedText2 = "test_encrypted_text_2";
            string encryptionMethod2 = "Caesar";

            // Сохраняем несколько зашифрованных текстов для теста
            _dbManager.SaveEncryptedText(encryptedText1, encryptionMethod1);
            _dbManager.SaveEncryptedText(encryptedText2, encryptionMethod2);

            // Act
            List<EncryptedRecord> records = _dbManager.GetAllEncryptedTexts();

            // Assert
            Assert.NotEmpty(records); // Проверяем, что список не пустой
            Assert.Contains(records, r => r.EncryptedText == encryptedText1);
            Assert.Contains(records, r => r.EncryptedText == encryptedText2);
        }

        [Fact]
        public void SaveEncryptedText_ShouldThrowException_WhenConnectionFails()
        {
            // Arrange
            string invalidConnectionString = "Host=invalid;Username=invalid;Password=invalid;Database=invalid";
            var invalidDbManager = new DatabaseManager(invalidConnectionString);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => invalidDbManager.SaveEncryptedText("text", "AES"));
            Assert.Contains("Ошибка при сохранении данных", exception.Message);
        }
    }
}
