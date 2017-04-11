using System;

namespace uCommunity.Subscribe
{
    public interface ISubscribableService
    {
        ISubscribable Subscribe(Guid userId, Guid activityId);

        void UnSubscribe(Guid userId, Guid activityId);

        void UpdateNotification(Guid id, bool value);
    }
}