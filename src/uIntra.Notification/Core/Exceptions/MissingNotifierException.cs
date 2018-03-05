using System;
using Uintra.Core.TypeProviders;
using Uintra.Notification.Configuration;

namespace Uintra.Notification.Exceptions
{
    public class MissingNotifierException : ApplicationException
    {
        public MissingNotifierException(NotifierTypeEnum notifierType, Enum notificationType)
            :base ($"Can not find notifier {notifierType} to notify by {notificationType.ToString()}")
        {
            
        }
    }
}