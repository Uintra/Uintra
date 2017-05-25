using System;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Exceptions
{
    public class MissingNotificationException : ApplicationException
    {
        public MissingNotificationException(NotificationTypeEnum notificationType)
            :base ($"Can not notify by {notificationType}, because it doesn't have any notifiers")
        {
            
        }
    }
}