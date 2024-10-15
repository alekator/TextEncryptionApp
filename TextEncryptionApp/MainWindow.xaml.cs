using System.Windows;
using TextEncryptionApp.Views;

namespace TextEncryptionApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            EncryptWindow encryptWindow = new EncryptWindow();
            encryptWindow.Show(); // Открыть окно шифрования
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            DecryptWindow decryptWindow = new DecryptWindow();
            decryptWindow.Show(); // Открыть окно дешифровки
        }
        private void DatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseViewerWindow databaseWindow = new DatabaseViewerWindow();
            databaseWindow.Show(); // Открыть окно базы данных
        }
    }
}
