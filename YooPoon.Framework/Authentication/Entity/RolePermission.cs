using YooPoon.Core.Data;

namespace YooPoon.Framework.Authentication.Entity
{
    public class RolePermission:IBaseEntity
    {
        public int Id { get; set; }

        public virtual Role Role { get; set; }

        public virtual ControllerAction ControllerAction { get; set; }

        public bool IsAllowed { get; set; }
    }
}