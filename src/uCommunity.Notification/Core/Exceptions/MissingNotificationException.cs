using System;
using uCommunity.Notification.Core.Configuration;

namespace uCommunity.Notification.Core.Exceptions
{
    public class MissingNotificationException : ApplicationException
    {
        public MissingNotificationException(NotificationTypeEnum notificationType)
            :base ($"Can not notify by {notificationType}, because it doesn't have any notifiers")
        {
            
        }
    }
}