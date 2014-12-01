using YooPoon.Core.Data;
using YooPoon.WebFramework.Authentication.Entity;

namespace YooPoon.WebFramework.User.Entity
{
    public class UserRole : IBaseEntity
    {
        public int Id { get; set; }

        public virtual UserBase User { get; set; }

        public virtual Role Role { get; set; }
    }
}