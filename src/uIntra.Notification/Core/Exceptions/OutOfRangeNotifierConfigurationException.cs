using System;
using Uintra.Notification.Configuration;

namespace Uintra.Notification.Exceptions
{
    public class OutOfRangeNotifierConfigurationException : ApplicationException
    {
        public OutOfRangeNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for {notifierType} is specified more than 1 times")
        {
            
        }
    }
}