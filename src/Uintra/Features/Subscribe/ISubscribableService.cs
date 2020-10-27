using System;
using Uintra.Core.Activity;

namespace Uintra.Features.Subscribe
{
    public interface ISubscribableService : ITypedService
    {
        ISubscribable Subscribe(Guid userId, Guid activityId);

        void UnSubscribe(Guid userId, Guid activityId);

        void UpdateNotification(Guid id, bool value);
    }
}