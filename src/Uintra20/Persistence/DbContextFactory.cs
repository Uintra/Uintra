using System.Data.Entity.Infrastructure;


namespace Uintra20.Persistence
{
    public class DbContextFactory : IDbContextFactory<DbObjectContext>
    {
        private readonly string connectionString;

        public DbContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DbObjectContext Create()
        {
            return new DbObjectContext(connectionString);
        }
    }
}