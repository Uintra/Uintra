using System.Data.Entity.Migrations;
using Uintra.Persistence.Context;

namespace Uintra.Persistence.Sql.Migrations
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
            
        }
    }
}