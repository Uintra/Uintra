using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Information;
using Uintra20.Features.Permissions.Implementation;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Caching;

namespace Uintra20.Infrastructure.Ioc
{
	public class UintraInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
			//configurations
			services.AddSingleton<IApplicationSettings, ApplicationSettings.ApplicationSettings>();

			//services
			services.AddSingleton<IInformationService,InformationService>();

            services.AddScoped<ICacheService, MemoryCacheService>();
            services.AddSingleton<IIntranetMemberGroupService, IntranetMemberGroupService>();

            return services;
		}
	}
}