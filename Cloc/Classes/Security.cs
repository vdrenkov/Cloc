using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Cloc.Classes
{
    internal static class Security
    {
        private readonly static string key = "b14ca5898a4e4133bbce2ea2315a1916";

        internal static string EncryptString(string plainText)
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

                    using MemoryStream memoryStream = new();
                    using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
                cipherText = Convert.ToBase64String(array);
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
                cipherText = string.Empty;
            }
            return cipherText;
        }

        internal static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];

            try
            {
                byte[] buffer = Convert.FromBase64String(cipherText);

                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new(buffer);
                using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new(cryptoStream);
                return streamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                ErrorLog.AddErrorLog(ex.ToString());
                MessageBox.Show(ex.Message);
                return string.Empty;
            }
        }

        internal static string HashString(string accessCode)
        {
            string hashedString;
            string salt = "sw>5;1.1,2gh3<:i";
            SHA256 sha256 = SHA256.Create();

            try
            {
                accessCode = string.Concat(salt, accessCode);
                byte[] accessCodeBytes = Encoding.UTF8.GetBytes(accessCode);
                byte[] hashBytes = sha256.ComputeHash(accessCodeBytes);
                hashedString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
            catch (Exception ex)
            {
                hashedString = string.Empty;
                ErrorLog.AddErrorLog(ex.ToString());
            }
            return hashedString;
        }
    }
}
