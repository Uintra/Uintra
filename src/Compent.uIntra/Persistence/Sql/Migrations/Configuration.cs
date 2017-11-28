using System.Data.Entity.Migrations;

namespace Compent.uIntra.Persistence.Sql.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<Compent.uIntra.Persistence.Sql.DbObjectContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Sql\Migrations";
        }

        protected override void Seed(Compent.uIntra.Persistence.Sql.DbObjectContext context)
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
