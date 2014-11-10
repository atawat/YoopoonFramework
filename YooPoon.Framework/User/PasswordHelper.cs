using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using YooPoon.Framework.User.Entity;

namespace YooPoon.Framework.User
{
    public class PasswordHelper
    {
        /// <summary>
        /// 设置加密密码
        /// </summary>
        /// <param name="userBase">用户实体</param>
        /// <param name="password">密码</param>
        /// <param name="hashName">加密算法(默认MD5)</param>
        public static void SetPasswordHashed(UserBase userBase, string password,string hashName = "MD5")
        {
            //随机生成加密密钥
            var saltBytes = new byte[0x10];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(saltBytes);
            }

            var passwordBytes = Encoding.Unicode.GetBytes(password);

            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes;
            using (var hashAlgorithm = HashAlgorithm.Create(hashName))
            {
                if(hashAlgorithm == null)
                    throw new Exception("不存在名为" + hashName + "的加密算法");
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }

            userBase.HashAlgorithm = hashName;
            userBase.Password = Convert.ToBase64String(hashBytes);
            userBase.PasswordSalt = Convert.ToBase64String(saltBytes);
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="userBase">用户实体</param>
        /// <param name="password">密码</param>
        /// <returns>是否通过</returns>
        public static bool ValidatePasswordHashed(UserBase userBase, string password)
        {

            var saltBytes = Convert.FromBase64String(userBase.PasswordSalt);

            var passwordBytes = Encoding.Unicode.GetBytes(password);

            var combinedBytes = saltBytes.Concat(passwordBytes).ToArray();

            byte[] hashBytes;
            using (var hashAlgorithm = HashAlgorithm.Create(userBase.HashAlgorithm))
            {
                hashBytes = hashAlgorithm.ComputeHash(combinedBytes);
            }

            return userBase.Password == Convert.ToBase64String(hashBytes);
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
