using System;
using System.Collections.Generic;
using uCommunity.Core.Activity;
using uCommunity.Core.Media;

namespace uCommunity.Events
{
    public interface IEventsService<in T, out TModel> : IIntranetActivityItemServiceBase<T, TModel>
            where T : EventBase
            where TModel : EventModelBase
    {
        IEnumerable<TModel> GetPastEvents();

        void Hide(Guid id);

        bool CanEditSubscribe(Guid activityId);

        bool CanSubscribe(EventBase activity);

        MediaSettings GetMediaSettings();
    }
}