using System;
using System.Collections.Generic;
using Uintra20.Core.Activity;
using Uintra20.Core.Activity.Entities;
using Uintra20.Features.Media;

namespace Uintra20.Features.Events
{
    public interface IEventsService<out TEvent> : IIntranetActivityService<TEvent> where TEvent : EventBase
    {
        IEnumerable<TEvent> GetPastEvents();

        IEnumerable<TEvent> GetComingEvents(DateTime fromDate);

        bool CanHide(Guid id);

        bool CanHide(IIntranetActivity activity);

        void Hide(Guid id);

        bool CanEditSubscribe(Guid activityId);

        bool CanSubscribe(Guid activityId);

        MediaSettings GetMediaSettings();
    }
}
