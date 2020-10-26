using Compent.Shared.DependencyInjection.Contract;
using Uintra.Core.Feed;
using Uintra.Core.Feed.Services;
using Uintra.Features.CentralFeed;
using Uintra.Features.CentralFeed.Builders;
using Uintra.Features.CentralFeed.Enums;
using Uintra.Features.CentralFeed.Helpers;
using Uintra.Features.CentralFeed.Providers;
using Uintra.Features.CentralFeed.Services;
using Uintra.Features.Groups.Services;
using Uintra.Features.Links;
using Uintra.Infrastructure.Grid;

namespace Uintra.Infrastructure.Ioc
{
    public class CentralFeedInjectModule:IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddSingleton<IFeedTypeProvider>(d => new CentralFeedTypeProvider(typeof(CentralFeedTypeEnum)));
            services.AddScoped<IFeedFilterService, FeedFilterService>();
			services.AddScoped<IGroupFeedService, GroupFeedService>();
            services.AddScoped<ICentralFeedHelper, CentralFeedHelper>();
            services.AddScoped<ICentralFeedService, CentralFeedService>();
            services.AddScoped<IFeedPresentationService, FeedPresentationService>();
            services.AddScoped<IFeedLinkService, ActivityLinkService>();
            services.AddScoped<ICentralFeedContentService, CentralFeedContentService>();
            services.AddScoped<ICentralFeedContentProvider, CentralFeedContentProvider>();
            services.AddScoped<IGridHelper, GridHelper>();
            services.AddScoped<IActivityTabsBuilder, ActivityTabsBuilder>();

            return services;
		}
	}
}