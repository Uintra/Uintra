using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Persistence;
using Uintra20.Persistence.Sql;

namespace Uintra20.Infrastructure.Ioc
{
	public class EntityFrameworkInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddTransient<IDbContextFactory<DbObjectContext>>(provider => new DbContextFactory("umbracoDbDSN"));
            services.AddScoped<DbContext>(provider => provider.GetService<IDbContextFactory<DbObjectContext>>().Create());
            services.AddTransient<IntranetDbContext, DbObjectContext>();
            services.AddTransient<Database>(provider => provider.GetService<DbObjectContext>().Database);
            services.AddTransient(typeof(ISqlRepository<,>), typeof(SqlRepository<,>));
            services.AddTransient(typeof(ISqlRepository<>), typeof(SqlRepository<>));

            return services;
		}
	}
}