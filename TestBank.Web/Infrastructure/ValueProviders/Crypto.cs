using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;

namespace TestBank.Web.Infrastructure.ValueProviders
{
    /*public static class Crypto
    {
        private static string encryptionKey = "!#s@3g`d";
        private static byte[] key = { };
        private static byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xdf, 0xeb };
        static RijndaelManaged GetCryptoMethod()
        {
            key = Encoding.UTF8.GetBytes(encryptionKey);
            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 8,
                BlockSize = 128,
                Key = key,
                IV = IV
            };

            return rijndaelManaged;
        }

        public static string Encrypt(Dictionary<string, string> keyValue)
        {
            byte[] queryString = Encoding.UTF8.GetBytes(StringFromDictionary(keyValue));
            return ToBase64(EncryptBytes(queryString));
        }

        public static Dictionary<string, string> Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = FromBase64(encryptedText);
            return DictionaryFromBytes(DecryptBytes(encryptedBytes));
        }

        static string ToBase64(byte[] bytes)
        {
            string strBase64;
            strBase64 = Convert.ToBase64String(bytes);
            return strBase64.Replace('+', '-').Replace('/', '_').Replace('=', ',');
        }

        static byte[] FromBase64(string encryptedText)
        {
            encryptedText = encryptedText.Replace('-', '+').Replace('_', '/').Replace(',', '=');
            return Convert.FromBase64String(encryptedText);
        }

        private static byte[] EncryptBytes(byte[] bytes)
        {
            return GetCryptoMethod().CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        }

        private static byte[] DecryptBytes(byte[] bytes)
        {
            return GetCryptoMethod().CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        }

        private static string StringFromDictionary(Dictionary<string, string> dictionary)
        {
            return string.Join("-", dictionary.Select(d => d.Key + "+" + d.Value));
        }

        private static Dictionary<string, string> DictionaryFromBytes(byte[] bytes)
        {
            string decryptedString = Encoding.UTF8.GetString(bytes);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] keyValuePair = decryptedString.Split('-');

            foreach (string key in keyValuePair)
            {
                string[] keyValue = key.Split('+');
                dictionary.Add(keyValue[0], keyValue[1]);
            }

            return dictionary;
        }
    }*/
}