using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Breadcrumbs.Services.Contracts;
using Uintra20.Features.Breadcrumbs.Services.Implementations;

namespace Uintra20.Features.Breadcrumbs.UnjectModules
{
    public class BreadcrumbInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IBreadcrumbService, BreadcrumbService>();

            return services;
        }
    }
}