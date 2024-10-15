# Text Encryption App

## Description
Text Encryption App is a text encryption and decryption application written in C# using WPF and PostgreSQL. The application allows users to select encryption methods, input text, and then encrypt it using the chosen method. It also provides functionality for saving the encrypted text to a database and sending it via email.

## Features
- Selection of encryption methods (e.g., Caesar cipher, AES, Vigenère cipher, Triple DES).
- Input text for encryption.
- Display of encrypted text.
- Saving encrypted text to a PostgreSQL database.
- Sending encrypted text via email.
- User interface built with WPF.

## Installation

### Requirements
- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- PostgreSQL
- Visual Studio or another C# IDE

### Installation Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/alekator/TextEncryptionApp.git
   cd TextEncryptionApp
2. Install the required NuGet packages:
dotnet restore
3. Configure the connection to the PostgreSQL database in the code (check the connection string).
4. Run the application:
dotnet run
### Usage
Launch the application.
Select the encryption method from the dropdown list.
Enter the text to be encrypted in the text box.
Click the "Encrypt" button to view the encrypted text.
Use the "Save" button to save the encrypted text to the database.
Use the "Send" button to send the encrypted text via email.
### License
This project is licensed under the MIT License. See the LICENSE file for details.

### Notes for Customization
For more information about the encryption methods used in the application, refer to the relevant files in the EncryptionMethods directory.
