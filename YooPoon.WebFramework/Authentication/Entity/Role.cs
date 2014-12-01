using System.Collections.Generic;
using YooPoon.Core.Data;

namespace YooPoon.WebFramework.Authentication.Entity
{
    public class Role : IBaseEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public RoleStatus Status { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; } 
    }
}