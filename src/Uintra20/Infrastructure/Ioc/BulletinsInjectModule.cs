using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Features.Bulletins;
using Uintra20.Features.Bulletins.Entities;
using Uintra20.Features.CentralFeed;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Infrastructure.Ioc
{
	public class BulletinsInjectModule: IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddScoped<INotifyableService, BulletinsService<Bulletin>>();
            services.AddScoped(typeof(ICacheableIntranetActivityService<Bulletin>), typeof(BulletinsService<Bulletin>));
            services.AddScoped<IFeedItemService, BulletinsService<Bulletin>>();
            services.AddScoped<IBulletinsService<Bulletin>, BulletinsService<Bulletin>>();

            return services;
		}
	}
}