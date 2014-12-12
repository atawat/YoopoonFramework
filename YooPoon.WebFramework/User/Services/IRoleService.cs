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
    }
}