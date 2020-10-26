using System;
using System.Collections.Generic;
using Uintra.Features.Notification.Configuration;

namespace Uintra.Features.Reminder.Services
{
    public interface IReminderService
    {
        void CreateIfNotExists(Guid activityId, ReminderTypeEnum type);
        void SetAsDelivered(Guid id);
        IEnumerable<Notification.Sql.Reminder> GetAllNotDelivered();
    }
}
