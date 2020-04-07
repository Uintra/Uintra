using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.LinkPreview.Mappers;
using Uintra20.Features.LinkPreview.Providers.Contracts;
using Uintra20.Features.LinkPreview.Providers.Implementations;

namespace Uintra20.Features.LinkPreview.InjectModules
{
    public class LinkPreviewInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<ILinkPreviewConfigProvider, LinkPreviewConfigProvider>();
            services.AddTransient(typeof(LinkPreviewModelMapper));

            return services;
        }
    }
}