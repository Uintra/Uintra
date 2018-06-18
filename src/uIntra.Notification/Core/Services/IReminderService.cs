using System;
using System.Collections.Generic;
using uIntra.Notification.Configuration;

namespace uIntra.Notification
{
    public interface IReminderService
    {
        Reminder CreateIfNotExists(Guid activityId, ReminderTypeEnum type);
        void SetAsDelivered(Guid id);
        IEnumerable<Reminder> GetAllNotDelivered();
    }
}
