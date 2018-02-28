using System;
using System.Collections.Generic;
using Uintra.Notification.Configuration;

namespace Uintra.Notification
{
    public interface IReminderService
    {
        Reminder CreateIfNotExists(Guid activityId, ReminderTypeEnum type);
        void SetAsDelivered(Guid id);
        IEnumerable<Reminder> GetAllNotDelivered();
    }
}
