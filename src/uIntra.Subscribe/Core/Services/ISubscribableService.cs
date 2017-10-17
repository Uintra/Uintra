using System;
using uIntra.Core.Activity;

namespace uIntra.Subscribe
{
    public interface ISubscribableService : ITypedService
    {
        ISubscribable Subscribe(Guid userId, Guid activityId);

        void UnSubscribe(Guid userId, Guid activityId);

        void UpdateNotification(Guid id, bool value);
    }
}