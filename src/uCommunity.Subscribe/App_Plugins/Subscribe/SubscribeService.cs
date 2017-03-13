using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Persistence.Sql;

namespace uCommunity.Subscribe
{
    public class SubscribeService : ISubscribeService
    {
        private readonly ISqlRepository<Subscribe> _subscribeRepository;

        public SubscribeService(ISqlRepository<Subscribe> subscribeRepository)
        {
            _subscribeRepository = subscribeRepository;
        }

        public Subscribe Get(Guid activityId, Guid userId)
        {
            return _subscribeRepository.Find(s => s.ActivityId == activityId && s.UserId == userId);
        }

        public IEnumerable<Subscribe> Get(Guid activityId)
        {
            return _subscribeRepository.FindAll(s => s.ActivityId == activityId);
        }

        public IEnumerable<Subscribe> GetByUserId(Guid userId)
        {
            return _subscribeRepository.FindAll(s => s.UserId == userId);
        }

        public bool IsSubscribed(Guid userId, Guid activityId)
        {
            return _subscribeRepository.Exists(s => s.UserId == userId && s.ActivityId == activityId);
        }

        public bool IsSubscribed(Guid userId, ISubscribable subscribers)
        {
            return subscribers.Subscribers.Any(s => s.UserId == userId);
        }

        public long GetVersion(Guid activityId)
        {
            var subscriber = _subscribeRepository.FindAll(subscribe => subscribe.ActivityId == activityId).OrderByDescending(subscribe => subscribe.CreatedDate).FirstOrDefault();
            return subscriber?.CreatedDate.Ticks ?? default(long);
        }

        public bool HasSubscribers(Guid activityId)
        {
            return _subscribeRepository.Exists(s => s.ActivityId == activityId);
        }

        public Subscribe Subscribe(Guid userId, Guid activityId)
        {
            var entity = new Subscribe
            {
                Id = Guid.NewGuid(),
                ActivityId = activityId,
                UserId = userId,
                CreatedDate = DateTime.Now,
                IsNotificationDisabled = true
            };

            _subscribeRepository.Add(entity);
            return entity;
        }

        public void Unsubscribe(Guid userId, Guid activityId)
        {
            _subscribeRepository.Delete(s => s.UserId == userId && s.ActivityId == activityId);
        }

        public void UpdateNotificationDisabled(Guid subscribeId, bool newValue)
        {
            var subscribe = _subscribeRepository.Get(subscribeId);
            subscribe.IsNotificationDisabled = newValue;
            _subscribeRepository.Update(subscribe);
        }

        public void FillSubscribers(ISubscribable entity)
        {
            entity.Subscribers = Get(entity.Id);
        }
    }
}