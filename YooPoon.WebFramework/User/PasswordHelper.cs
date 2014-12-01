using YooPoon.Common.Encryption;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.User
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
            string saltString;
            var hashstring = EncrypHelper.HashEncrypt(password, hashName, out saltString);

            userBase.HashAlgorithm = hashName;
            userBase.Password = hashstring;
            userBase.PasswordSalt = saltString;
        }

        /// <summary>
        /// 验证密码
        /// </summary>
        /// <param name="userBase">用户实体</param>
        /// <param name="password">密码</param>
        /// <returns>是否通过</returns>
        public static bool ValidatePasswordHashed(UserBase userBase, string password)
        {
            return EncrypHelper.ValidateHashValue(password, userBase.PasswordSalt, userBase.Password, userBase.HashAlgorithm);
        }
    }
}
