using System;
using System.Collections.Generic;
using Uintra20.Features.Notification.Configuration;

namespace Uintra20.Features.Reminder
{
    public interface IReminderService
    {
        Notification.Sql.Reminder CreateIfNotExists(Guid activityId, ReminderTypeEnum type);
        void SetAsDelivered(Guid id);
        IEnumerable<Notification.Sql.Reminder> GetAllNotDelivered();
    }
}
