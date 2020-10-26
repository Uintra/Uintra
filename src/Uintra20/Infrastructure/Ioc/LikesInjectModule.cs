using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Features.Likes.Services;

namespace Uintra20.Infrastructure.Ioc
{
	public class LikesInjectModule : IInjectModule
	{
		public IDependencyCollection Register(IDependencyCollection services)
		{
            services.AddScoped<ILikesService, LikesService>();

            return services;
		}
	}
}