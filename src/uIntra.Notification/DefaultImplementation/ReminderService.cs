using System;
using System.Collections.Generic;
using Uintra.Core.Persistence;
using Uintra.Notification.Configuration;
using Uintra.Notification.Exceptions;

namespace Uintra.Notification
{
    public class ReminderService : IReminderService
    {
        private readonly ISqlRepository<Reminder> _remindeRepository;

        public ReminderService(ISqlRepository<Reminder> remindeRepository)
        {
            _remindeRepository = remindeRepository;
        }

        public Reminder CreateIfNotExists(Guid activityId, ReminderTypeEnum type)
        {
            if (_remindeRepository.Exists(r => r.ActivityId == activityId && r.Type == type && !r.IsDelivered))
            {
                return null;
            }

            var entity = new Reminder
            {
                Id = Guid.NewGuid(),
                ActivityId = activityId,
                Type = type
            };

            _remindeRepository.Add(entity);
            return entity;
        }

        public void SetAsDelivered(Guid id)
        {
            var entity = _remindeRepository.Get(id);
            if (entity.IsDelivered)
            {
                throw new ReminderIsAlreadyDeliveredException(id);
            }

            entity.IsDelivered = true;
            _remindeRepository.Update(entity);
        }

        public IEnumerable<Reminder> GetAllNotDelivered()
        {
            return _remindeRepository.FindAll(r => !r.IsDelivered);
        }
    }
}
