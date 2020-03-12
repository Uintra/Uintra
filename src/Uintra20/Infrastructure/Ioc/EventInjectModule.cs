using Compent.Shared.DependencyInjection.Contract;
using Uintra20.Core.Activity;
using Uintra20.Core.Feed.Services;
using Uintra20.Features.Events;
using Uintra20.Features.Events.Entities;
using Uintra20.Features.Notification.Services;

namespace Uintra20.Infrastructure.Ioc
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

            return services;
        }
    }
}