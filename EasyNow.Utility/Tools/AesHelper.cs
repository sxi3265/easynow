using System;
using System.Security.Cryptography;
using System.Text;

namespace EasyNow.Utility.Tools
{
    /// <summary>
    /// Aes工具类
    /// </summary>
    public static class AesHelper
    {
        /// <summary>
        /// 使用CBC模式进行加密
        /// </summary>
        /// <param name="rawInput"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string CbcEncrypt(string rawInput, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(rawInput))
            {
                return string.Empty;
            }

            if (key == null || iv == null || key.Length < 1 || iv.Length < 1)
            {
                throw new ArgumentException("Key/Iv is null.");
            }

            using (var rijndaelManaged = new RijndaelManaged {
                Key = key, // 密钥，长度可为128， 196，256比特位
                IV = iv,  //初始化向量(Initialization vector), 用于CBC模式初始化
                KeySize = 256,//接受的密钥长度
                BlockSize = 128,//加密时的块大小，应该与iv长度相同
                Mode = CipherMode.CBC,//加密模式
                Padding = PaddingMode.PKCS7 }) //填白模式，对于AES, C# 框架中的 PKCS　＃７等同与Java框架中 PKCS #5
            {
                using (var transform = rijndaelManaged.CreateEncryptor(key, iv))
                {
                    var inputBytes = Encoding.UTF8.GetBytes(rawInput);//字节编码， 将有特等含义的字符串转化为字节流
                    var encryptedBytes = transform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);//加密
                    return Convert.ToBase64String(encryptedBytes);//将加密后的字节流转化为字符串，以便网络传输与储存。
                }
            }
        }
        
        /// <summary>
        /// 使用CBC模式进行解密
        /// </summary>
        /// <param name="encryptedInput"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string CbcDecrypt(string encryptedInput, byte[] key, byte[] iv)
        {
            if (string.IsNullOrEmpty(encryptedInput))
            {
                return string.Empty;
            }

            if (key == null || iv == null || key.Length < 1 || iv.Length < 1)
            {
                throw new ArgumentException("Key/Iv is null.");
            }

            using (var rijndaelManaged = new RijndaelManaged { Key = key, IV = iv, KeySize = 256,
                BlockSize = 128, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7})
            {
                using (var transform = rijndaelManaged.CreateDecryptor(key, iv))
                {
                    var inputBytes = Convert.FromBase64String(encryptedInput);
                    var encryptedBytes = transform.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                    return Encoding.UTF8.GetString(encryptedBytes);
                }
            }
        }
    }
}