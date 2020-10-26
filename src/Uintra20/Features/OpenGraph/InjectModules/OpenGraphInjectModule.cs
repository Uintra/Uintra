using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.OpenGraph.Services.Contracts;
using Uintra20.Features.OpenGraph.Services.Implementations;

namespace Uintra20.Features.OpenGraph.InjectModules
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