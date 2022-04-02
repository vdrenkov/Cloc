using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cloc.Classes
{
    internal class Security
    {
        private readonly static string key = "b14ca5898a4e4133bbce2ea2315a1916";

        public static string EncryptString(string plainText)
        {
            string cipherText;
            byte[] iv = new byte[16];
            byte[] array;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream memoryStream = new ();
                    using CryptoStream cryptoStream = new ((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new ((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
                cipherText = Convert.ToBase64String(array);
            }
            catch (Exception)
            {
                cipherText = null;
            }
            return cipherText;
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];

            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new (buffer);
                using CryptoStream cryptoStream = new((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new((Stream)cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch(Exception)
            {
                return null;
            }
        }

        public static string HashString(string accessCode)
        {
            string hashedString;
            string salt = "sw>5;1.1,2gh3<:i";
            SHA256 sha256 = SHA256.Create();

            try
            {
                accessCode = String.Concat(salt, accessCode);
                byte[] accessCodeBytes = Encoding.UTF8.GetBytes(accessCode);
                byte[] hashBytes = sha256.ComputeHash(accessCodeBytes);
                hashedString = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
            catch (Exception)
            {
                hashedString = null;
                MessageBox.Show("Възникна грешка при обработката на вашия код за достъп.");
            }
            return hashedString;
        }
    }
}
