using System;
using uCommunity.Notification.Notifier;

namespace uCommunity.Notification.Exceptions
{
    public class OutOfRangeNotifierConfigurationException : ApplicationException
    {
        public OutOfRangeNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for {notifierType} is specified more than 1 times")
        {
            
        }
    }
}