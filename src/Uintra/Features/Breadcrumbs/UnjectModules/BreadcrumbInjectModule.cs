using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.Breadcrumbs.Services.Contracts;
using Uintra.Features.Breadcrumbs.Services.Implementations;

namespace Uintra.Features.Breadcrumbs.UnjectModules
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