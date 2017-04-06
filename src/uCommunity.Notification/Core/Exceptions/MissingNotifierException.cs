using System;
using uCommunity.Notification.Core.Configuration;

namespace uCommunity.Notification.Core.Exceptions
{
    public class MissingNotifierException : ApplicationException
    {
        public MissingNotifierException(NotifierTypeEnum notifierType, NotificationTypeEnum notificationType)
            :base ($"Can not find notifier {notifierType} to notify by {notificationType}")
        {
            
        }
    }
}