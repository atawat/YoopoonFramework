using System;
using System.Collections.Generic;
using System.Linq;
using YooPoon.Core.Data;
using YooPoon.Core.Logging;
using YooPoon.WebFramework.Authentication.Entity;

namespace YooPoon.WebFramework.Authentication.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<Role> _roleRepository;
        private readonly ILog _log;

        public RoleService(IRepository<Role> roleRepository, ILog log)
        {
            _roleRepository = roleRepository;
            _log = log;
        }

        public Role GetRoleById(int id)
        {
            try
            {
                return _roleRepository.GetById(id);
            }
            catch (Exception e)
            {
                _log.Error(e, "获取角色出错");
                return null;
            }
        }

        public IEnumerable<Role> ListRoles()
        {
            try
            {
                return _roleRepository.Table.ToList();
            }
            catch (Exception e)
            {
                _log.Error(e, "无法获取角色列表");
                return null;
            }
        }

        public bool ModifyRole(Role role)
        {
            try
            {
                _roleRepository.Update(role);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "修改角色出错");
                return false;
            }
        }

        public Role CreateRole(Role role)
        {
            try
            {
                _roleRepository.Insert(role);
                return role;
            }
            catch (Exception e)
            {
                _log.Error(e, "创建角色错误");
                return null;
            }
        }

        public bool DeleteRole(Role role)
        {
            try
            {
                _roleRepository.Delete(role);
                return true;
            }
            catch (Exception e)
            {
                _log.Error(e, "删除角色错误");
                return false;
            }
        }
    }
}