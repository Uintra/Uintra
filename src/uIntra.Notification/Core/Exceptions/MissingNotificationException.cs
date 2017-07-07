using System;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Exceptions
{
    public class MissingNotificationException : ApplicationException
    {
        public MissingNotificationException(IIntranetType notificationType)
            :base ($"Can not notify by {notificationType.Name}, because it doesn't have any notifiers")
        {
            
        }
    }
}