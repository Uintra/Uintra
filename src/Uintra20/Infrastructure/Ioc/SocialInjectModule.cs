using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Notification.Services;

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

            return services;
		}
	}
}