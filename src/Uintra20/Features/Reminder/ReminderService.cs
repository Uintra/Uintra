using System;
using System.Collections.Generic;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Persistence.Sql;

namespace Uintra20.Features.Reminder
{
    public class ReminderService : IReminderService
    {
        private readonly ISqlRepository<Notification.Sql.Reminder> _reminderRepository;

        public ReminderService(ISqlRepository<Notification.Sql.Reminder> reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public Notification.Sql.Reminder CreateIfNotExists(Guid activityId, ReminderTypeEnum type)
        {
            if (_reminderRepository.Exists(r => r.ActivityId == activityId && r.Type == type && !r.IsDelivered))
            {
                return null;
            }

            var entity = new Notification.Sql.Reminder
            {
                Id = Guid.NewGuid(),
                ActivityId = activityId,
                Type = type
            };

            _reminderRepository.Add(entity);
            return entity;
        }

        public void SetAsDelivered(Guid id)
        {
            var entity = _reminderRepository.Get(id);
            if (entity.IsDelivered)
            {
                throw new ReminderIsAlreadyDeliveredException(id);
            }

            entity.IsDelivered = true;
            _reminderRepository.Update(entity);
        }

        public IEnumerable<Notification.Sql.Reminder> GetAllNotDelivered()
        {
            return _reminderRepository.FindAll(r => !r.IsDelivered);
        }
    }
}
