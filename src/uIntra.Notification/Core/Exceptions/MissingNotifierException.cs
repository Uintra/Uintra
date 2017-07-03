using System;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Exceptions
{
    public class MissingNotifierException : ApplicationException
    {
        public MissingNotifierException(NotifierTypeEnum notifierType, IIntranetType notificationType)
            :base ($"Can not find notifier {notifierType} to notify by {notificationType.Name}")
        {
            
        }
    }
}