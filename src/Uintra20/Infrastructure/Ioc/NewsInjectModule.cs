using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.News;
using Uintra20.Features.News.Entities;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Infrastructure.Ioc
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