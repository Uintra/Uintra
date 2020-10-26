using Compent.Shared.DependencyInjection.Contract;
using Uintra.Core.Activity;
using Uintra.Core.Feed.Services;
using Uintra.Features.News;
using Uintra.Features.News.Entities;
using Uintra.Features.Notification.Services;

namespace Uintra.Infrastructure.Ioc
{
    public class NewsInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScopedToCollection<IFeedItemService, NewsService>();
            services.AddScoped<INewsService<News>, NewsService>();
            services.AddScopedToCollection<INotifyableService, NewsService>();
            services.AddScoped<ICacheableIntranetActivityService<News>, NewsService>();
            services.AddScoped<IIntranetActivityService, NewsService>();

            return services;
        }
    }
}