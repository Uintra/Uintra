using System.Data.Entity.Migrations;

namespace Uintra20.Persistence.Sql.Migrations
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