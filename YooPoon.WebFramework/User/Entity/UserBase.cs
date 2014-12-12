using System;
using System.Collections.Generic;
using YooPoon.Core.Data;

namespace YooPoon.WebFramework.User.Entity
{
    public class UserBase : IBaseEntity,IUser
    {
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 标准化用户名(小写)
        /// </summary>
        public string NormalizedName { get; set; }

        /// <summary>
        /// 密码(加密后)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 加密算法
        /// </summary>
        public string HashAlgorithm { get; set; }

        /// <summary>
        /// 加密密钥
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}