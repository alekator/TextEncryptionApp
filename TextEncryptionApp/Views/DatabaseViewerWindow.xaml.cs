using System;
using System.Collections.Generic;
using System.Windows;
using TextEncryptionApp.Models;

namespace TextEncryptionApp.Views
{
    public partial class DatabaseViewerWindow : Window
    {
        private DatabaseManager dbManager;

        public DatabaseViewerWindow()
        {
            InitializeComponent();
            dbManager = new DatabaseManager("Host=127.0.0.1;Username=admin;Password=admin;Database=TextEncryption");
            LoadDatabaseEntries();
        }

        private void LoadDatabaseEntries()
        {
            try
            {
                var records = dbManager.GetAllEncryptedTexts();
                dataGrid.ItemsSource = records;
            }
            catch (Exception ex)
            {
                // Выводим сообщение об ошибке с детальной информацией
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}\n{ex.StackTrace}");
            }
        }


        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadDatabaseEntries(); // Перезагружаем данные при нажатии на кнопку
        }
    }
}
