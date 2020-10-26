﻿using System;
using Uintra.Features.Notification.Configuration;

namespace Uintra.Features.Notification.Exceptions
{
    public class MissingNotifierConfigurationException : ApplicationException
    {
        public MissingNotifierConfigurationException(NotifierTypeEnum notifierType)
            : base($"Configuration for notifier {notifierType} is missing")
        {

        }
    }
}