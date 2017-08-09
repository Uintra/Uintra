using System.Data.Entity;

namespace uIntra.Core.Persistence
{
    public abstract class IntranetDbContext : DbContext
    {
        public IntranetDbContext() : this("umbracoDbDSN")
        {
        }

        public IntranetDbContext(string nameOrConnectionString)
              : base(nameOrConnectionString)
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }
    }
}
