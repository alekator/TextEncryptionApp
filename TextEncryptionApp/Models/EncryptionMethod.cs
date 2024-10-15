using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEncryptionApp.Models
{
     public interface IEncryptionMethod
    {
        string Name { get; }
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }

}
