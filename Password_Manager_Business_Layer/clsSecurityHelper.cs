using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Password_Manager_Business_Layer
{
    public class clsSecurityHelper
    {
        private byte[] _PrivateKey;
        public clsSecurityHelper(string UserName)
        {
            _PrivateKey = _GenerateKey(UserName);
        }
        private byte[] _GenerateKey(string username)
        {
            byte[] salt = Encoding.UTF8.GetBytes("JustMyUniqueRandomSalt");

            using (var deriveBytes = new Rfc2898DeriveBytes(username, salt, 10000))
            {
                return deriveBytes.GetBytes(32);
            }
        }
        public string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _PrivateKey;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                    }
                    array = memoryStream.ToArray();
                }
            }
            return Convert.ToBase64String(array);
        }
        public string Decrypt(string cipherText)
        {

            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = _PrivateKey;
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        public static string Decrypt(string password, string username)
        {
            clsSecurityHelper sh = new clsSecurityHelper(username);
            string decPass = sh.Decrypt(password);
            return decPass;
        }
        public static string Encrypt(string password, string username)
        {
            clsSecurityHelper sh = new clsSecurityHelper(username);
            string Encrypt = sh.Encrypt(password);
            return Encrypt;
        }
    }
}
