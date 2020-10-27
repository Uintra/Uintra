using Compent.Shared.DependencyInjection.Contract;
using Uintra.Core.Activity;
using Uintra.Core.Feed.Services;
using Uintra.Features.Notification.Services;
using Uintra.Features.Social;
using Uintra.Features.Social.Entities;

namespace Uintra.Infrastructure.Ioc
{
	public class SocialInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
			services.AddScopedToCollection<INotifyableService, SocialService<Social>>();
            services.AddScoped<ICacheableIntranetActivityService<Social>, SocialService<Social>>();
            services.AddScoped<IFeedItemService, SocialService<Social>>();
            services.AddScoped<ISocialService<Social>, SocialService<Social>>();
            services.AddScoped<IIntranetActivityService, SocialService<Social>>();

            return services;
		}
	}
}