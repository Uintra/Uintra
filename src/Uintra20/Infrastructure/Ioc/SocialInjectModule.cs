using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.Notification.Services;
using Uintra20.Features.Social;
using Uintra20.Features.Social.Entities;

namespace Uintra20.Infrastructure.Ioc
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