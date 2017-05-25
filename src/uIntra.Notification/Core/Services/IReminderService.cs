using System;
using System.Collections.Generic;
using uIntra.Notification.Core.Configuration;
using uIntra.Notification.Core.Sql;

namespace uIntra.Notification.Core.Services
{
    public interface IReminderService
    {
        Reminder CreateIfNotExists(Guid activityId, ReminderTypeEnum type);
        void SetAsDelivered(Guid id);
        IEnumerable<Reminder> GetAllNotDelivered();
    }
}
