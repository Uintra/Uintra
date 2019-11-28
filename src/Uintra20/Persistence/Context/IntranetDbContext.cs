using System.Data.Entity;

namespace Uintra20.Persistence.Context
{
    public abstract class IntranetDbContext : DbContext
    {
        protected IntranetDbContext() : this("umbracoDbDSN")
        {
        }

        protected IntranetDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    }
}