using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.OpenGraph.Services.Contracts;
using Uintra.Features.OpenGraph.Services.Implementations;

namespace Uintra.Features.OpenGraph.InjectModules
{
    public class OpenGraphInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IOpenGraphService, OpenGraphService>();

            return services;
        }
    }
}