using System.Data.Entity.Migrations;

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
