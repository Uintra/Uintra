﻿using System;
using System.Collections.Generic;
using System.Linq;
using Uintra.Core.Configuration;
using Uintra.Notification.Exceptions;
using static LanguageExt.Prelude;

namespace Uintra.Notification.Configuration
{
    public class NotificationConfigurationProvider : ConfigurationProvider<NotificationConfiguration>
    {
        public NotificationConfigurationProvider(string settingsFilePath)
            : base(settingsFilePath)
        {
        }

        protected override void AssertConfigurationIsValid()
        {
            var exceptions = GetConfigurationExceptions().ToList();
            if (exceptions.Count == 0)
            {
                return;
            }

            throw new AggregateException(exceptions);
        }

        private IEnumerable<ApplicationException> GetConfigurationExceptions()
        {
            var configuration = GetSettings();
            var notifierTypes = configuration.NotificationTypeConfigurations
                .SelectMany(nc => nc.NotifierTypes)
                .Concat(List(configuration.DefaultNotifier))
                .Distinct();

            foreach (var notifierType in notifierTypes)
            {
                var count = configuration.NotifierConfigurations.Count(nc => nc.NotifierType == notifierType);
                if (count == 0)
                {
                    yield return new MissingNotifierConfigurationException(notifierType);
                    continue;
                }

                if (count > 1)
                {
                    yield return new OutOfRangeNotifierConfigurationException(notifierType);
                }
            }
        }
    }
}
