using System;
using System.Collections.Generic;

namespace uCommunity.Notification.Core.Services
{
    public interface IUiNotifierService : INotifierService
    {
        IEnumerable<Sql.Notification> GetMany(Guid receiverId, int count, out int totalCount);
        int GetNotNotifiedCount(Guid receiverId);
        void ViewNotification(Guid id);
    }
}
