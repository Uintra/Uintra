using Compent.LinkPreview.Core;
using Compent.LinkPreview.HttpClient;
using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.LinkPreview.Configurations;
using Uintra20.Features.LinkPreview.Mappers;
using Uintra20.Features.LinkPreview.Providers.Contracts;
using Uintra20.Features.LinkPreview.Providers.Implementations;
using Uintra20.Features.LinkPreview.Services;

namespace Uintra20.Features.LinkPreview.InjectModules
{
    public class LinkPreviewInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<ILinkPreviewClient, LinkPreviewClient>();
            services.AddScoped<ILinkPreviewConfiguration, LinkPreviewConfiguration>();
            services.AddScoped<ILinkPreviewUriProvider, LinkPreviewUriProvider>();
            services.AddScoped<ILinkPreviewConfigProvider, LinkPreviewConfigProvider>();
            services.AddScoped<LinkPreviewModelMapper>();
            services.AddScoped<IActivityLinkPreviewService, ActivityLinkPreviewService>();
            services.AddScoped<ClientConnection>();

            return services;
        }
    }
}