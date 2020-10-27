using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.Navigation.ApplicationSettings;
using Uintra.Features.Navigation.Builders;
using Uintra.Features.Navigation.Helpers;
using Uintra.Features.Navigation.Services;
using Uintra.Infrastructure.UintraInformation;

namespace Uintra.Features.Navigation.InjectModules
{
    public class NavigationInjectModule: IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<INavigationModelsBuilder, NavigationModelsBuilder>();
            services.AddScoped<IUintraInformationService, UintraInformationService>();
            services.AddScoped<IMyLinksHelper, MyLinksHelper>();
            services.AddScoped<ISubNavigationModelBuilder, SubNavigationModelBuilder>();

            services.AddSingleton<INavigationApplicationSettings, NavigationApplicationSettings>();

            return services;
        }
    }
}