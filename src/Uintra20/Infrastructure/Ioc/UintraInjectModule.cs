using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Information;
using Uintra20.Infrastructure.ApplicationSettings;

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

			return services;
		}
	}
}