using Compent.Shared.DependencyInjection.Contract;
using Uintra.Features.Likes.Services;

namespace Uintra.Infrastructure.Ioc
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