using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.CentralFeed.Links;
using Uintra20.Features.Groups.Links;
using Uintra20.Features.Links;
using Uintra20.Features.Navigation.Services;

namespace Uintra20.Infrastructure.Ioc
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