using System.Data.Entity.Migrations;

namespace YooPoon.Data.EntityFramework.Migrations
{
    public class EfConfiguration:DbMigrationsConfiguration<EfDbContext>
    {
        public EfConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;  //如果数据迁移时会发生数据丢失，false则抛出异常，true不抛出异常
        }
    }
}
