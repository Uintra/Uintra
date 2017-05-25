using System;
using uIntra.Notification.Core.Configuration;

namespace uIntra.Notification.Core.Exceptions
{
    public class OutOfRangeNotifierConfigurationException : ApplicationException
    {
        public OutOfRangeNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for {notifierType} is specified more than 1 times")
        {
            
        }
    }
}