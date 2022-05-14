using System;
using System.Security.Cryptography;
using System.Text;

namespace Snowball.Core.Utils
{
    public static class SecurityUtil
    {
        /// <summary>
        /// SHA1签名
        /// </summary>
        /// <param name="input">被签名的字符串</param>
        /// <returns></returns>
        public static string Sha1(string input)
        {
            if (input == null)
            {
                return null;
            }

            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            string byte2String = null;
            for (int i = 0; i < hash.Length; i++)
            {
                byte2String += hash[i].ToString("x2");
            }

            return byte2String;
        }

        public static string AesEncrypt(string input, string encodingAESKey)
        {
            if (input == null)
            {
                return null;
            }

            var inputBuffer = Encoding.UTF8.GetBytes(input);
            byte[] keyBuffer = Convert.FromBase64String(encodingAESKey);
            byte[] ivBuffer = new byte[16];
            Array.Copy(keyBuffer, ivBuffer, 16);
            return AesEncrypt(inputBuffer, keyBuffer, ivBuffer);
        }

        public static string AesEncrypt(string input, string key, string iv)
        {
            if (input == null)
            {
                return null;
            }

            var inputBuffer = Encoding.UTF8.GetBytes(input);
            byte[] keyBuffer = Convert.FromBase64String(key);
            byte[] ivBuffer = Convert.FromBase64String(iv);
            return AesEncrypt(inputBuffer, keyBuffer, ivBuffer);
        }

        public static string AesEncrypt(byte[] input, byte[] key, byte[] iv)
        {
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            aes.Key = key;
            aes.IV = iv;
            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedBuffer = encryptor.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(encryptedBuffer);
        }

        public static byte[] AesDecrypt(string encrypted, string encodingAESKey)
        {
            if (encrypted == null)
            {
                return null;
            }

            byte[] encryptedBuffer = Convert.FromBase64String(encrypted);
            byte[] keyBuffer = Convert.FromBase64String(encodingAESKey);
            byte[] ivBuffer = new byte[16];
            Array.Copy(keyBuffer, ivBuffer, 16);
            return AesDecrypt(encryptedBuffer, keyBuffer, ivBuffer);
        }

        public static byte[] AesDecrypt(string encrypted, string key, string iv)
        {
            if (encrypted == null)
            {
                return null;
            }

            byte[] encryptedBuffer = Convert.FromBase64String(encrypted);
            byte[] keyBuffer = Convert.FromBase64String(key);
            byte[] ivBuffer = Convert.FromBase64String(iv);
            return AesDecrypt(encryptedBuffer, keyBuffer, ivBuffer);
        }

        public static byte[] AesDecrypt(byte[] encrypted, byte[] key, byte[] iv)
        {
            var aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.None;
            aes.Key = key;
            aes.IV = iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            return decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
        }
    }
}
