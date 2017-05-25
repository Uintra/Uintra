using System;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Exceptions
{
    public class MissingNotifierConfigurationException : ApplicationException
    {
        public MissingNotifierConfigurationException(NotifierTypeEnum notifierType)
            :base ($"Configuration for notifier {notifierType} is missing")
        {
            
        }
    }
}