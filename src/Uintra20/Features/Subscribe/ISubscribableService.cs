using System;
using Uintra20.Core.Activity;

namespace Uintra20.Features.Subscribe
{
    public interface ISubscribableService : ITypedService
    {
        ISubscribable Subscribe(Guid userId, Guid activityId);

        void UnSubscribe(Guid userId, Guid activityId);

        void UpdateNotification(Guid id, bool value);
    }
}