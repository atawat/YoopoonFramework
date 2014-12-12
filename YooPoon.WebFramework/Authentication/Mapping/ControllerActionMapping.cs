using System.Data.Entity.ModelConfiguration;
using YooPoon.Core.Data;
using YooPoon.WebFramework.Authentication.Entity;

namespace YooPoon.WebFramework.Authentication.Mapping
{
    public class ControllerActionMapping : EntityTypeConfiguration<ControllerAction>,IMapping
    {
        public ControllerActionMapping()
        {
            ToTable("ControllerAction");
            HasKey(c => c.Id);
            Property(c => c.ActionName);
            Property(c => c.ControllerName);
        }
    }
}