using System;
using Uintra20.Features.Notification.Configuration;

namespace Uintra20.Features.Notification.Exceptions
{
    public class OutOfRangeNotifierConfigurationException : ApplicationException
    {
        public OutOfRangeNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for {notifierType} is specified more than 1 times")
        {

        }
    }
}