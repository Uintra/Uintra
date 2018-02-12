using System;
using Uintra.Notification.Configuration;

namespace Uintra.Notification.Exceptions
{
    public class MissingNotifierConfigurationException : ApplicationException
    {
        public MissingNotifierConfigurationException(NotifierTypeEnum notifierType)
            :base ($"Configuration for notifier {notifierType} is missing")
        {
            
        }
    }
}