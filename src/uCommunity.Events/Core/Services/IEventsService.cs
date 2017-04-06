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

        bool CanEditSubscribe(EventBase activity);

        bool CanSubscribe(EventBase activity);

        bool HasSubscribers(EventBase activity);

        MediaSettings GetMediaSettings();
    }
}