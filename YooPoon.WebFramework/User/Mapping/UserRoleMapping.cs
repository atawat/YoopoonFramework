using System.Data.Entity.ModelConfiguration;
using YooPoon.Core.Data;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.User.Mapping
{
    public class UserRoleMapping : EntityTypeConfiguration<UserRole>, IMapping
    {
        public UserRoleMapping()
        {
            ToTable("User_Role");
            HasKey(c => c.Id);
            HasRequired(c => c.Role);
        }
    }
}