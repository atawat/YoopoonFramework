using System.Data.Entity.ModelConfiguration;
using YooPoon.Core.Data;
using YooPoon.WebFramework.Authentication.Entity;

namespace YooPoon.WebFramework.Authentication.Mapping
{
    public class RolePermissionMapping : EntityTypeConfiguration<RolePermission>,IMapping
    {
        public RolePermissionMapping()
        {
            ToTable("Role_Permission");
            HasKey(c => c.Id);
            Property(c => c.IsAllowed);

            HasRequired(c => c.Role);
            HasRequired(c => c.ControllerAction);
        }
    }
}