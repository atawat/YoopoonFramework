using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace YooPoon.Common.Encryption
{
    public class EncrypHelper
    {
        /// <summary>
        /// 验证字符串是否与哈希加密的字符串相等
        /// </summary>
        /// <param name="validValue">待验证字符串</param>
        /// <param name="salt">密钥（Base64）</param>
        /// <param name="targetValue">已加密的字符串</param>
        /// <param name="hashAlgorithmMethod">加密算法</param>
        /// <returns></returns>
        public static bool ValidateHashValue(string validValue, string salt, string targetValue, string hashAlgorithmMethod)
        {
            var saltBytes = Convert.FromBase64String(salt);

            var validBytes = Encoding.Unicode.GetBytes(validValue);

            var combinedBytes = saltBytes.Concat(validBytes).ToArray();

            byte[] hashBytes;
            using (var hashAlgorithm = HashAlgorithm.Create(hashAlgorithmMethod))
            {
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }
            return targetValue == Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 哈希加密法
        /// </summary>
        /// <param name="sourceString">要加密的字符串</param>
        /// <param name="hashName">加密方法</param>
        /// <param name="saltString">密钥</param>
        /// <returns></returns>
        public static string HashEncrypt(string sourceString, string hashName, out string saltString)
        {
            //随机生成加密密钥
            var saltBytes = new byte[0x10];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(saltBytes);
            }

            var passwordBytes = Encoding.Unicode.GetBytes(sourceString);

            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes;
            using (var hashAlgorithm = HashAlgorithm.Create(hashName))
            {
                if (hashAlgorithm == null)
                {
                    throw new Exception("不存在名为" + hashName + "的加密算法");
                }
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }
            saltString = Convert.ToBase64String(saltBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Encrypt(string sourceString, string key)
        {
            try
            {
                byte[] btKey = Convert.FromBase64String(key).Take(8).ToArray();

                byte[] btIV = Convert.FromBase64String(key).Reverse().Take(8).ToArray();

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (
                            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV),
                                CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);

                            cs.FlushFinalBlock();
                        }

                        return HttpUtility.UrlEncode(Convert.ToBase64String(ms.ToArray()));
                    }
                    catch
                    {
                        return sourceString;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedString, string key)
        {
            byte[] btKey = Convert.FromBase64String(key).Take(8).ToArray();

            byte[] btIV = Convert.FromBase64String(key).Reverse().Take(8).ToArray(); ;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return encryptedString;
                }
            }
        }  
    }
}