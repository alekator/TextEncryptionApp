using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TextEncryptionApp.EncryptionMethods;
using TextEncryptionApp.Models;

namespace TextEncryptionApp.Views
{
    public partial class EncryptWindow : Window
    {
        private List<IEncryptionMethod> encryptionMethods;

        public EncryptWindow()
        {
            InitializeComponent();
            LoadEncryptionMethods();
            encryptionMethodComboBox.ItemsSource = encryptionMethods;
            encryptionMethodComboBox.SelectionChanged += EncryptionMethodComboBox_SelectionChanged; // Обработка смены метода
        }

        private void LoadEncryptionMethods()
        {
            encryptionMethods = new List<IEncryptionMethod>
            {
                new CaesarCipher(),
                new AesEncryption(),
                new VigenereCipher("KEY"), // Временное значение ключа
                new TripleDESAlgorithm("DEFAULT_KEY")
            };
        }

        private void EncryptionMethodComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedMethod = encryptionMethodComboBox.SelectedItem as IEncryptionMethod;
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


        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedMethod = encryptionMethodComboBox.SelectedItem as IEncryptionMethod;
            if (selectedMethod != null)
            {
                string plainText = inputTextBox.Text;
                string encryptedText = string.Empty;

                // Проверяем, если выбран шифр Виженера
                if (selectedMethod is VigenereCipher vigenereCipher)
                {
                    if (!string.IsNullOrWhiteSpace(keyTextBox.Text))
                    {
                        // Создаем новый экземпляр шифра Виженера с введенным ключом
                        vigenereCipher = new VigenereCipher(keyTextBox.Text);
                        encryptedText = vigenereCipher.Encrypt(plainText);
                        outputTextBox.Text = encryptedText;
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, введите ключ.");
                        return; // Прекращаем выполнение, если ключ не введен
                    }
                }
                else
                {
                    encryptedText = selectedMethod.Encrypt(plainText);
                    outputTextBox.Text = encryptedText;
                }

                // Сохранение зашифрованного текста в базе данных
                try
                {
                    DatabaseManager dbManager = new DatabaseManager("Host=myserver;Username=mylogin;Password=mypass;Database=mydatabase");

                    // Сохраняем зашифрованный текст, метод шифрования и текущую дату
                    int id = dbManager.SaveEncryptedText(encryptedText, selectedMethod.Name);

                    MessageBox.Show($"Сообщение зашифровано и сохранено с ID: {id}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении в базу данных: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите метод шифрования.");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что текст для сохранения не пуст
            if (string.IsNullOrWhiteSpace(outputTextBox.Text))
            {
                MessageBox.Show("Сначала зашифруйте текст, чтобы его сохранить.");
                return;
            }

            // Открываем диалоговое окно для выбора места сохранения файла
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Сохранить зашифрованное сообщение"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    // Сохраняем зашифрованный текст в выбранный файл
                    System.IO.File.WriteAllText(saveFileDialog.FileName, outputTextBox.Text);
                    MessageBox.Show("Сообщение успешно сохранено!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}");
                }
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, что текст для отправки не пуст
            if (string.IsNullOrWhiteSpace(outputTextBox.Text))
            {
                MessageBox.Show("Сначала зашифруйте текст, чтобы его отправить.");
                return;
            }

            // Запрашиваем адрес электронной почты у пользователя
            string recipientEmail = Microsoft.VisualBasic.Interaction.InputBox("Введите адрес электронной почты получателя:", "Отправить по почте");

            // Проверка, введен ли адрес
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                MessageBox.Show("Адрес электронной почты не может быть пустым.");
                return;
            }

            try
            {
                // Настройки SMTP
                var smtpClient = new System.Net.Mail.SmtpClient("smtp.your-email-provider.com") // Замените на свой SMTP-сервер
                {
                    Port = 587, // Обычно 587 для TLS
                    Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-email-password"), // Введите ваши данные
                    EnableSsl = true,
                };

                // Создание сообщения
                var mailMessage = new System.Net.Mail.MailMessage
                {
                    From = new System.Net.Mail.MailAddress("your-email@example.com"),
                    Subject = "Зашифрованное сообщение",
                    Body = outputTextBox.Text,
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(recipientEmail);

                // Отправка сообщения
                smtpClient.Send(mailMessage);
                MessageBox.Show("Сообщение успешно отправлено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }


    }
}
