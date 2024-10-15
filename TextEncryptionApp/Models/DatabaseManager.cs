using Npgsql;
using System;
using System.Collections.Generic;

namespace TextEncryptionApp.Models
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Метод для сохранения зашифрованного текста
        public int SaveEncryptedText(string encryptedText, string encryptionMethod)
        {
            int newId;
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки данных в таблицу
                    var sql = "INSERT INTO EncryptedMessages (EncryptedText, EncryptionMethod, CreatedAt) VALUES (@encryptedText, @encryptionMethod, @createdAt) RETURNING Id";
                    using (var command = new NpgsqlCommand(sql, connection))
                    {
                        // Добавляем параметры в запрос
                        command.Parameters.AddWithValue("encryptedText", encryptedText);
                        command.Parameters.AddWithValue("encryptionMethod", encryptionMethod);
                        command.Parameters.AddWithValue("createdAt", DateTime.Now);

                        // Выполняем запрос и получаем сгенерированный ID
                        newId = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку при сохранении
                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}\n{ex.StackTrace}");
                throw; // Пробрасываем исключение дальше, если нужно
            }
            return newId;
        }


        // Метод для получения всех зашифрованных сообщений из базы данных
        public List<EncryptedRecord> GetAllEncryptedTexts()
        {
            List<EncryptedRecord> records = new List<EncryptedRecord>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT id, encryptedText, encryptionMethod, createdAt FROM EncryptedMessages";

                using (var command = new NpgsqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var record = new EncryptedRecord
                        {
                            Id = reader.GetInt32(0),
                            EncryptedText = reader.GetString(1),
                            EncryptionMethod = reader.GetString(2),
                            EncryptionDate = reader.GetDateTime(3)
                        };
                        records.Add(record);
                    }
                }
            }

            return records;
        }
    }

    // Модель данных для зашифрованных записей
    public class EncryptedRecord
    {
        public int Id { get; set; }
        public string EncryptedText { get; set; }
        public string EncryptionMethod { get; set; }
        public DateTime EncryptionDate { get; set; }
    }
}
