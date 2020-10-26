using Compent.Shared.DependencyInjection.Contract;
using Uintra.Core.Activity;
using Uintra.Core.Feed.Services;
using Uintra.Features.Events;
using Uintra.Features.Events.Entities;
using Uintra.Features.Notification.Services;
using Uintra.Features.Subscribe;

namespace Uintra.Infrastructure.Ioc
{
    public class EventInjectModule : IInjectModule
    {
        public IDependencyCollection Register(IDependencyCollection services)
        {
            services.AddScoped<IEventsService<Event>, EventsService>();
            services.AddScopedToCollection<IFeedItemService, EventsService>();
            services.AddScopedToCollection<INotifyableService, EventsService>();
            services.AddScoped<ICacheableIntranetActivityService<Event>, EventsService>();
            services.AddScoped<IIntranetActivityService, EventsService>();
            services.AddScoped<ISubscribableService, EventsService>();

            return services;
        }
    }
}