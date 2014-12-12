using System.Data.Entity.ModelConfiguration;
using YooPoon.Core.Data;
using YooPoon.WebFramework.User.Entity;

namespace YooPoon.WebFramework.User.Mapping
{
    public class UserMapping : EntityTypeConfiguration<UserBase>, IMapping
    {
        public UserMapping()
        {
            ToTable("User");
            HasKey(c => c.Id);
            Property(c => c.Status).HasColumnType("int");
            Property(c => c.RegTime).HasColumnType("datetime");
            Property(c => c.UserName).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.NormalizedName).HasColumnType("varchar").HasMaxLength(50);
            Property(c => c.Password).HasColumnType("varchar").HasMaxLength(255);
            Property(c => c.HashAlgorithm).HasColumnType("varchar").HasMaxLength(255);
            Property(c => c.PasswordSalt).HasColumnType("varchar").HasMaxLength(255);

            HasMany(c => c.UserRoles).WithRequired(s => s.User);
        }
    }
}