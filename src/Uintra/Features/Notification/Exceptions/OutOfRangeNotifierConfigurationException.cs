using System;
using Uintra.Features.Notification.Configuration;

namespace Uintra.Features.Notification.Exceptions
{
    public class OutOfRangeNotifierConfigurationException : ApplicationException
    {
        public OutOfRangeNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for {notifierType} is specified more than 1 times")
        {

        }
    }
}