using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.CentralFeed.Links;
using Uintra.Features.Groups.Links;
using Uintra.Features.Links;
using Uintra.Features.Navigation.Services;

namespace Uintra.Infrastructure.Ioc
{
    public class LinksInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddTransient<ICentralFeedLinkProvider, CentralFeedLinkProvider>();
            services.AddTransient<IGroupFeedLinkProvider, GroupFeedLinkProvider>();
            services.AddTransient<IActivityLinkService, ActivityLinkService>();
            services.AddTransient<IFeedLinkService, ActivityLinkService>();
            services.AddScoped<IMyLinksService, MyLinksService>();
            services.AddScoped<IProfileLinkProvider, ProfileLinkProvider>();
            services.AddScoped<IErrorLinksService, ErrorLinksService>();

            return services;
        }
    }
}