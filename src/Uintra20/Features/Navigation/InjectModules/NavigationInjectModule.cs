using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Navigation.ApplicationSettings;
using Uintra20.Features.Navigation.Builders;
using Uintra20.Features.Navigation.Helpers;
using Uintra20.Features.Navigation.Services;
using Uintra20.Infrastructure.UintraInformation;

namespace Uintra20.Features.Navigation.InjectModules
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