using System.Collections.Generic;
using YooPoon.Core.Autofac;
using YooPoon.WebFramework.Authentication.Entity;

namespace YooPoon.WebFramework.User.Services
{
    public interface IRoleService:IDependency
    {
        Role GetRoleById(int id);

        IEnumerable<Role> ListRoles();

        bool ModifyRole(Role role);

        Role CreateRole(Role role);

        bool DeleteRole(Role role);

        /// <summary>
        /// 查询该名字的规则是否存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Role GetRoleByName(string roleName);
    }
}