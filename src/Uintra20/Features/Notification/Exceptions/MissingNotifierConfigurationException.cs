using System;
using Uintra20.Features.Notification.Configuration;

namespace Uintra20.Features.Notification.Exceptions
{
    public class MissingNotifierConfigurationException : ApplicationException
    {
        public MissingNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for notifier {notifierType} is missing")
        {

        }
    }
}