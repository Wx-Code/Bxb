using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Shunmai.Bxb.Utilities.Encryption
{
    /// <summary>
    /// AES 加密工具类
    /// </summary>
    /// <author>谭光洪</author>
    /// <since>2018-11-13</since>
    public static class AESHelper
    {
        private static string Decrypt(string encryptedStr, ICryptoTransform decryptor)
        {
            var encryptBytes = Convert.FromBase64String(encryptedStr);
            using (var msDecrypt = new MemoryStream(encryptBytes))
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }

        /// <summary>
        /// 采用算法 AES-128-CBC 对给定字符串进行解密
        /// </summary>
        /// <param name="encryptedStr"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AES128CBCDecrypt(string encryptedStr, string key, string iv)
        {
            using (var rijndaelManaged = new RijndaelManaged
            {
                KeySize = 128,
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC,
                Key = Convert.FromBase64String(key),
                IV = Convert.FromBase64String(iv)
            })
            using (var decryptor = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV))
            {
                return Decrypt(encryptedStr, decryptor);
            }
        }

        /// <summary>
        /// 采用 AES-128-ECB 算法对给定字符串进行加密，返回加密后的密文
        /// </summary>
        /// <param name="source">待加密的明文字符串</param>
        /// <param name="key">加密时采用的密钥</param>
        /// <returns>采用 AES-128-ECB 算法加密后的密文</returns>
        public static string AES128ECBEncrypt(string source, string key)
        {
            ObjectUtils.EnsureNotNullOrEmpty(source, nameof(source));
            ObjectUtils.EnsureNotNullOrEmpty(key, nameof(key));

            using (var rijndaelManaged = new RijndaelManaged
            {
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.ECB,
                Key = Encoding.UTF8.GetBytes(key.PadRight(32))
            })
            using (var encryptor = rijndaelManaged.CreateEncryptor())
            {
                var toEncryptArray = Encoding.UTF8.GetBytes(source);
                var encryptBytes = encryptor.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(encryptBytes);
            }
        }

        /// <summary>
        /// 采用 AES-128-ECB 算法对给定字符串进行解密
        /// </summary>
        /// <param name="encryptedStr">采用 AES-128-ECB 算法加密过的密文</param>
        /// <param name="key">加密时采用的密钥</param>
        /// <returns>采用 AES-128-ECB 算法解密后的明文</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static string AES128ECBDecrypt(string encryptedStr, string key)
        {
            ObjectUtils.EnsureNotNullOrEmpty(encryptedStr, nameof(encryptedStr));
            ObjectUtils.EnsureNotNullOrEmpty(key, nameof(key));
            
            using (var aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);

                using (var decryptor = aes.CreateDecryptor())
                {
                    return Decrypt(encryptedStr, decryptor);
                }
            }
        }
    }
}
