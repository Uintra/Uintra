using System;
using System.Collections.Generic;
using uCommunity.Core.Activity;
using uCommunity.Core.Media;

namespace uCommunity.Events
{
    public interface IEventsService : IIntranetActivityService<EventBase>
    {
        IEnumerable<EventBase> GetPastEvents();

        void Hide(Guid id);

        bool CanEditSubscribe(Guid activityId);

        bool CanSubscribe(EventBase activity);

        MediaSettings GetMediaSettings();
    }
}