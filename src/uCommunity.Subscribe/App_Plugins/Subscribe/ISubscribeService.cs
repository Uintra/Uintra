using System;
using System.Collections.Generic;

namespace uCommunity.Subscribe
{
    public interface ISubscribeService
    {
        Subscribe Get(Guid activityId, Guid userId);

        IEnumerable<Subscribe> Get(Guid activityId);

        IEnumerable<Subscribe> GetByUserId(Guid userId);

        bool IsSubscribed(Guid userId, Guid activityId);

        bool IsSubscribed(Guid userId, ISubscribable subscribers);

        long GetVersion(Guid activityId);

        bool HasSubscribers(Guid activityId);

        Subscribe Subscribe(Guid userId, Guid activityId);

        void Unsubscribe(Guid userId, Guid activityId);

        void UpdateNotificationDisabled(Guid subscribeId, bool newValue);

        void FillSubscribers(ISubscribable entity);
    }
}
