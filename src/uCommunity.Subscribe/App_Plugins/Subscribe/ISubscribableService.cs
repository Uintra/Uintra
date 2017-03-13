using System;

namespace uCommunity.Subscribe
{
    public interface ISubscribableService
    {
        Subscribe Subscribe(Guid userId, Guid activityId);

        void UnSubscribe(Guid userId, Guid activityId);

        bool CanSubscribe(ISubscribable entity);
    }
}