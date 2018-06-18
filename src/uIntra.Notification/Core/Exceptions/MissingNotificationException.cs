using System;

namespace Uintra.Notification.Exceptions
{
    public class MissingNotificationException : ApplicationException
    {
        public MissingNotificationException(Enum notificationType)
            :base ($"Can not notify by {notificationType.ToString()}, because it doesn't have any notifiers")
        {
            
        }
    }
}