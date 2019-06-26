using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Activity;
using Uintra.Core.Persistence;

namespace Uintra.Subscribe
{
    public class SubscribeService : ISubscribeService
    {
        private readonly ISqlRepository<Subscribe> _subscribeRepository;

        public SubscribeService(ISqlRepository<Subscribe> subscribeRepository)
        {
            _subscribeRepository = subscribeRepository;
        }

        public virtual Subscribe Get(Guid activityId, Guid userId)
        {
            return _subscribeRepository.Find(s => s.ActivityId == activityId && s.UserId == userId);
        }

        public virtual IEnumerable<Subscribe> Get(Guid activityId)
        {
            return _subscribeRepository.FindAll(s => s.ActivityId == activityId);
        }

        public virtual IEnumerable<Subscribe> GetByUserId(Guid userId)
        {
            return _subscribeRepository.FindAll(s => s.UserId == userId);
        }

        public virtual bool IsSubscribed(Guid userId, Guid activityId)
        {
            return _subscribeRepository.Exists(s => s.UserId == userId && s.ActivityId == activityId);
        }

        public virtual bool IsSubscribed(Guid userId, ISubscribable subscribers)
        {
            return subscribers.Subscribers.Any(s => s.UserId == userId);
        }

        public virtual long GetVersion(Guid activityId)
        {
            var subscriber = _subscribeRepository.FindAll(subscribe => subscribe.ActivityId == activityId).OrderByDescending(subscribe => subscribe.CreatedDate).FirstOrDefault();
            return subscriber?.CreatedDate.Ticks ?? default(long);
        }

        public virtual bool HasSubscribers(Guid activityId)
        {
            return _subscribeRepository.Exists(s => s.ActivityId == activityId);
        }

        public virtual Subscribe Subscribe(Guid userId, Guid activityId)
        {
            var entity = new Subscribe
            {
                Id = Guid.NewGuid(),
                ActivityId = activityId,
                UserId = userId,
                CreatedDate = DateTime.UtcNow,
                IsNotificationDisabled = true
            };

            _subscribeRepository.Add(entity);
            return entity;
        }

        public virtual void Unsubscribe(Guid userId, Guid activityId)
        {
            _subscribeRepository.Delete(s => s.UserId == userId && s.ActivityId == activityId);
        }

        public virtual Subscribe UpdateNotification(Guid subscribeId, bool newValue)
        {
            var subscribe = _subscribeRepository.Get(subscribeId);
            subscribe.IsNotificationDisabled = newValue;
            _subscribeRepository.Update(subscribe);
            return subscribe;
        }

        public virtual bool HasNotification(Enum type)
        {
            return type is IntranetActivityTypeEnum.Events;
        }

        public virtual void FillSubscribers(ISubscribable entity)
        {
            entity.Subscribers = Get(entity.Id);
        }
    }
}