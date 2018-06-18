using System;
using System.Collections.Generic;
using uIntra.Core.Activity;
using uIntra.Core.Media;

namespace uIntra.Events
{
    public interface IEventsService<out TEvent> : IIntranetActivityService<TEvent> where TEvent : EventBase
    {
        IEnumerable<TEvent> GetPastEvents();

        IEnumerable<TEvent> GetComingEvents(DateTime fromDate);

        void Hide(Guid id);

        bool CanEditSubscribe(Guid activityId);

        bool CanSubscribe(Guid activityId);

        MediaSettings GetMediaSettings();
    }
}