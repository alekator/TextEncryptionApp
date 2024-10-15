using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TextEncryptionApp.EncryptionMethods;
using TextEncryptionApp.Models;

namespace TextEncryptionApp.Views
{
    public partial class DecryptWindow : Window
    {
        private List<IEncryptionMethod> decryptionMethods;

        public DecryptWindow()
        {
            InitializeComponent();
            LoadDecryptionMethods();
            decryptionMethodComboBox.ItemsSource = decryptionMethods;
            decryptionMethodComboBox.SelectionChanged += DecryptionMethodComboBox_SelectionChanged; // Обработка смены метода
        }

        private void LoadDecryptionMethods()
        {
            decryptionMethods = new List<IEncryptionMethod>
            {
                new CaesarCipher(),
                new AesEncryption(),
                new VigenereCipher("KEY"), // Временное значение ключа
                new TripleDESAlgorithm("DEFAULT_KEY")
            };
        }

        private void DecryptionMethodComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedMethod = decryptionMethodComboBox.SelectedItem as IEncryptionMethod;
            if (selectedMethod is VigenereCipher || selectedMethod is TripleDESAlgorithm) // Проверяем, если метод требует ключ
            {
                keyLabel.Visibility = Visibility.Visible;  // Показываем поле для ключа
                keyTextBox.Visibility = Visibility.Visible;
            }
            else
            {
                keyLabel.Visibility = Visibility.Collapsed; // Скрываем поле для ключа
                keyTextBox.Visibility = Visibility.Collapsed;
            }
        }


        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMethod = decryptionMethodComboBox.SelectedItem as IEncryptionMethod;
            if (selectedMethod != null)
            {
                if (selectedMethod is VigenereCipher vigenereCipher)
                {
                    if (!string.IsNullOrWhiteSpace(keyTextBox.Text))
                    {
                        vigenereCipher = new VigenereCipher(keyTextBox.Text); // Новый экземпляр с введенным ключом
                        string encryptedText = encryptedTextBox.Text;
                        string decryptedText = vigenereCipher.Decrypt(encryptedText);
                        outputTextBox.Text = decryptedText;
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, введите ключ.");
                    }
                }
                else
                {
                    string encryptedText = encryptedTextBox.Text;
                    string decryptedText = selectedMethod.Decrypt(encryptedText);
                    outputTextBox.Text = decryptedText;
                }
            }
            else
            {
                MessageBox.Show("Выберите метод расшифровки.");
            }
        }
    }
}
