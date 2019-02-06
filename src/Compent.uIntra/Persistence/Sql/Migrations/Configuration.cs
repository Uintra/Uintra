using System;
using System.Data.Entity.Migrations;
using Uintra.Core.Permissions;
using Uintra.Core.Permissions.Sql;

namespace Compent.Uintra.Persistence.Sql.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<DbObjectContext>
    {
        public Configuration()
        {
            SetSqlGenerator("System.Data.SqlClient", new DefaultValueSqlServerMigrationSqlGenerator());
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Sql\Migrations";
        }

        protected override void Seed(DbObjectContext context)
        {
            //  This method will be called after migrating to the latest version.

            context.PermissionActivityTypes.AddOrUpdate(
                pact=>pact.Activity,
                new PermissionActivityTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Activity = PermissionActivityTypeEnum.News.ToString()
                },
                new PermissionActivityTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Activity = PermissionActivityTypeEnum.Events.ToString()
                },
                new PermissionActivityTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Activity = PermissionActivityTypeEnum.Bulletins.ToString()
                },
                new PermissionActivityTypeEntity()
                {
                    Id = Guid.NewGuid(),
                    Activity = PermissionActivityTypeEnum.Common.ToString()
                });


            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
