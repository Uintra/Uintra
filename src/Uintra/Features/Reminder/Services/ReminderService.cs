using System;
using System.Collections.Generic;
using Uintra.Features.Notification.Configuration;
using Uintra.Features.Reminder.Exceptions;
using Uintra.Persistence.Sql;

namespace Uintra.Features.Reminder.Services
{
    public class ReminderService : IReminderService
    {
        private readonly ISqlRepository<Notification.Sql.Reminder> _reminderRepository;

        public ReminderService(ISqlRepository<Notification.Sql.Reminder> reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public void CreateIfNotExists(Guid activityId, ReminderTypeEnum type)
        {
            if (_reminderRepository.Exists(r => r.ActivityId == activityId && r.Type == type))
            {
                return;
            }

            var entity = new Notification.Sql.Reminder
            {
                Id = Guid.NewGuid(),
                ActivityId = activityId,
                Type = type
            };

            _reminderRepository.Add(entity);
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
