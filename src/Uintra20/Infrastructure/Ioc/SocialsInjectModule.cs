using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Infrastructure.Ioc
{
	public class SocialsInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddScoped<INotifyableService, SocialsService<Social>>();
            services.AddScoped<ICacheableIntranetActivityService<Social>, SocialsService<Social>>();
            services.AddScoped<IFeedItemService, SocialsService<Social>>();
            services.AddScoped<ISocialsService<Social>, SocialsService<Social>>();

            return services;
		}
	}
}