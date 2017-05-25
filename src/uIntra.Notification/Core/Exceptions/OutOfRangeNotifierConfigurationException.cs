using System;
using uCommunity.Notification.Core.Configuration;

namespace uCommunity.Notification.Core.Exceptions
{
    public class OutOfRangeNotifierConfigurationException : ApplicationException
    {
        public OutOfRangeNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for {notifierType} is specified more than 1 times")
        {
            
        }
    }
}