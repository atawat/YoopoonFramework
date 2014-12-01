using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace YooPoon.Data.EntityFramework.Migrations
{
    public class EfInitializer<TContext, TMigrationsConfiguration> : IDatabaseInitializer<TContext>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private readonly DbMigrationsConfiguration config;

        /// <summary>
        ///     Initializes a new instance of the MigrateDatabaseToLatestVersion class.
        /// </summary>
        public EfInitializer()
        {
            config = new TMigrationsConfiguration();
        }

        /// <summary>
        ///     Initializes a new instance of the MigrateDatabaseToLatestVersion class that will
        ///     use a specific connection string from the configuration file to connect to
        ///     the database to perform the migration.
        /// </summary>
        /// <param name="connectionString"> connection string to use for migration. </param>
        public EfInitializer(string connectionString)
        {
            config = new TMigrationsConfiguration
            {
                TargetDatabase = new DbConnectionInfo(connectionString, "System.Data.SqlClient")
            };
        }

        public void InitializeDatabase(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("上下文不能为空");
            }

            var migrator = new DbMigrator(config);

            migrator.Update();
        }
    }
}
