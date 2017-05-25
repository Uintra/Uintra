using System;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Exceptions
{
    public class MissingNotificationException : ApplicationException
    {
        public MissingNotificationException(NotificationTypeEnum notificationType)
            :base ($"Can not notify by {notificationType}, because it doesn't have any notifiers")
        {
            
        }
    }
}