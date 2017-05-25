using System;
using System.Collections.Generic;
using uIntra.Core.Activity;
using uIntra.Core.Media;

namespace uIntra.Events
{
    public interface IEventsService<out TEvent> : IIntranetActivityService<TEvent> where TEvent: EventBase
    {
        IEnumerable<TEvent> GetPastEvents();

        void Hide(Guid id);

        bool CanEditSubscribe(Guid activityId);

        bool CanSubscribe(EventBase activity);

        MediaSettings GetMediaSettings();
    }
}