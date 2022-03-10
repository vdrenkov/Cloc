using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cloc.Classes
{
    internal class Hasher
    {
        public static string HashKey(string key)
        {
            string salt = "sw>5;1.1,2gh3<:i";
            SHA256 sha256 = SHA256.Create();

            key = String.Concat(salt, key);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] hashBytes = sha256.ComputeHash(keyBytes);
            string hashedKey = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

            return hashedKey;
        }
    }
}
