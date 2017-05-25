using System;
using System.Collections.Generic;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Sql;

namespace uCommunity.Notification.Core.Services
{
    public interface IReminderService
    {
        Reminder CreateIfNotExists(Guid activityId, ReminderTypeEnum type);
        void SetAsDelivered(Guid id);
        IEnumerable<Reminder> GetAllNotDelivered();
    }
}
