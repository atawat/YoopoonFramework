using YooPoon.Core.Data;

namespace YooPoon.Framework.Authentication.Entity
{
    public class RolePermission:IBaseEntity
    {
        public int Id { get; set; }

        public Role Role { get; set; }

        public ControllerAction ControllerAction { get; set; }

        public bool IsAllowed { get; set; }
    }
}