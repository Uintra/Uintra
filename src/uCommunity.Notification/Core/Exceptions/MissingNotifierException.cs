using System;
using uCommunity.Notification.Notifier;

namespace uCommunity.Notification.Exceptions
{
    public class MissingNotifierException : ApplicationException
    {
        public MissingNotifierException(NotifierTypeEnum notifierType, NotificationTypeEnum notificationType)
            :base ($"Can not find notifier {notifierType} to notify by {notificationType}")
        {
            
        }
    }
}