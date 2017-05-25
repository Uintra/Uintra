using System;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Exceptions
{
    public class MissingNotifierException : ApplicationException
    {
        public MissingNotifierException(NotifierTypeEnum notifierType, NotificationTypeEnum notificationType)
            :base ($"Can not find notifier {notifierType} to notify by {notificationType}")
        {
            
        }
    }
}