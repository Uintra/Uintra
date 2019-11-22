using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.CentralFeed.Enums;
using Uintra20.Features.Links;

namespace Uintra20.Infrastructure.Ioc
{
	public class CentralFeedInjectModule:IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
			services.AddScoped<IFeedFilterStateService<FeedFiltersState>, CentralFeedFilterStateService>();

			services.AddSingleton<IFeedTypeProvider>(d => new CentralFeedTypeProvider(typeof(CentralFeedTypeEnum)));


			//services.AddScoped<IFeedItemService, NewsService>();
			//services.AddScoped<IFeedItemService, EventsService>();
			services.AddScopedToCollection<IFeedItemService,BulletinsService<Bulletin>>();
			
			services.AddScoped<IFeedFilterService, FeedFilterService>();
			services.AddScoped<ICentralFeedService, CentralFeedService>();
			services.AddScoped<IFeedLinkService, ActivityLinkService>();

			return services;
		}
	}
}