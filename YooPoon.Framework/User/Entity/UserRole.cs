using YooPoon.Core.Data;
using YooPoon.Framework.Authentication.Entity;

namespace YooPoon.Framework.User.Entity
{
    public class UserRole : IBaseEntity
    {
        public int Id { get; set; }

        public virtual UserBase User { get; set; }

        public virtual Role Role { get; set; }
    }
}