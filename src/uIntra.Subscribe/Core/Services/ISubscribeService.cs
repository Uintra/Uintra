using System;
using System.Collections.Generic;
using uIntra.Core.Activity;

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

        Subscribe UpdateNotification(Guid subscribeId, bool newValue);

        bool HasNotification(IntranetActivityTypeEnum type);

        void FillSubscribers(ISubscribable entity);
    }
}
