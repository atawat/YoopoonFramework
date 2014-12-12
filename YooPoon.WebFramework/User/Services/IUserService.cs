using System.Linq;
using YooPoon.Core.Autofac;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.User.Services
{
    public interface IUserService : IDependency
    {
        /// <summary>
        /// 增加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserBase InsertUser(UserBase user);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool DeleteUser(UserBase user);
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserBase FindUser(int id);
        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool ModifyUser(UserBase user);

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        /// <param name="usernameOrEmail"></param>
        /// <returns></returns>
        UserBase GetUserByName(string usernameOrEmail);

        IQueryable<UserBase> GetUserByCondition(UserSearchCondition condition);

        int GetUserCountByCondition(UserSearchCondition condition);
    }
}