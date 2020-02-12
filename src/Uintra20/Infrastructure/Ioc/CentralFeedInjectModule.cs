using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Feed;
using Uintra20.Core.Feed.Services;
using Uintra20.Core.Feed.State;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.CentralFeed.Builders;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.CentralFeed.Providers;
using Uintra20.Features.CentralFeed.Services;
using Uintra20.Features.CentralFeed.State;
using Uintra20.Features.Groups.Services;
using Uintra20.Features.Links;
using Uintra20.Infrastructure.Grid;

namespace Uintra20.Infrastructure.Ioc
{
    public class CentralFeedInjectModule:IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
			services.AddScoped<IFeedFilterStateService<FeedFiltersState>, CentralFeedFilterStateService>();

			services.AddSingleton<IFeedTypeProvider>(d => new CentralFeedTypeProvider(typeof(CentralFeedTypeEnum)));

			services.AddScoped<IFeedFilterService, FeedFilterService>();
			services.AddScoped<IGroupFeedService, GroupFeedService>();
			services.AddScoped<ICentralFeedService, CentralFeedService>();
			services.AddScoped<IFeedLinkService, ActivityLinkService>();
            services.AddScoped<ICentralFeedContentService, CentralFeedContentService>();
            services.AddScoped<ICentralFeedContentProvider, CentralFeedContentProvider>();
            services.AddScoped<IGridHelper, GridHelper>();

            services.AddScoped<CentralFeedHub>();
			services.AddScoped<IActivityTabsBuilder, ActivityTabsBuilder>();

            return services;
		}
	}
}