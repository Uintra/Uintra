using System;
using uCommunity.Notification.Core.Configuration;

namespace uCommunity.Notification.Core.Exceptions
{
    public class MissingNotifierConfigurationException : ApplicationException
    {
        public MissingNotifierConfigurationException(NotifierTypeEnum notifierType)
            :base ($"Configuration for notifier {notifierType} is missing")
        {
            
        }
    }
}