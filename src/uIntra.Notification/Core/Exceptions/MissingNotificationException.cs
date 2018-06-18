using System;
using uIntra.Core.TypeProviders;

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