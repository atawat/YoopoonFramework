using System.Data.Entity.ModelConfiguration;
using YooPoon.Core.Data;
using YooPoon.WebFramework.Authentication.Entity;

namespace YooPoon.WebFramework.Authentication.Mapping
{
    public class RoleMapping : EntityTypeConfiguration<Role>, IMapping
    {
        public RoleMapping()
        {
            ToTable("Role");
            HasKey(c => c.Id);
            Property(c => c.RoleName);
            Property(c => c.Status);
            Property(c => c.Description);

            HasMany(c => c.RolePermissions).WithRequired(r => r.Role);
        }
    }
}
