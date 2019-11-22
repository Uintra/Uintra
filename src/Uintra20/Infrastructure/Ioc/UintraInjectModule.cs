using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.Information;
using Uintra20.Features.Permissions;
using Uintra20.Features.Permissions.Implementation;
using Uintra20.Features.Permissions.Interfaces;
using Uintra20.Features.Permissions.TypeProviders;
using Uintra20.Infrastructure.ApplicationSettings;
using Uintra20.Infrastructure.Caching;
using Uintra20.Infrastructure.Providers;

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
			services.AddScoped<IPermissionsService, PermissionsService>();

			services.AddSingleton<IPermissionResourceTypeProvider>(d => new PermissionActivityTypeProvider(typeof(PermissionResourceTypeEnum)));

			services.AddScoped<ICookieProvider, CookieProvider>();

			services.AddSingleton<IApplicationSettings, ApplicationSettings.ApplicationSettings>();

			services.AddScoped<ICacheService, MemoryCacheService>();

			return services;
		}
	}
}