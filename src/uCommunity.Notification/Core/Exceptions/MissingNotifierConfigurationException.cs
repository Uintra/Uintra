using System;
using uCommunity.Notification.Notifier;

namespace uCommunity.Notification.Exceptions
{
    public class MissingNotifierConfigurationException : ApplicationException
    {
        public MissingNotifierConfigurationException(NotifierTypeEnum notifierType)
            :base ($"Configuration for notifier {notifierType} is missing")
        {
            
        }
    }
}