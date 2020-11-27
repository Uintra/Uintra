using System;
using Uintra.Features.Events.Entities;

namespace Uintra.Features.Events.Extensions
{
    public static class EventsExtensions
    {
        public static bool IsCacheable(this Event @event) => !IsEventHidden(@event) && IsActualPublishDate(@event);

        public static bool IsActualPublishDate(this Event @event) => DateTime.Compare(@event.PublishDate, DateTime.UtcNow) <= 0;

        public static bool IsEventHidden(this Event @event) => @event == null || @event.IsHidden;
    }
}