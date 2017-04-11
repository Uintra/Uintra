using System;
using System.Collections.Generic;
using System.Linq;
using uCommunity.Core.Configuration;
using uCommunity.Core.Exceptions;
using uCommunity.Core.Extentions;
using uCommunity.Notification.Core.Exceptions;
using uCommunity.Notification.Core.Services;
using uCommunity.Notification.Core.Configuration;
using uCommunity.Notification.Core.Entities;

namespace uCommunity.Notification
{
    public class NotificationsService : INotificationsService
    {
        private readonly IEnumerable<INotifierService> _notifiers;
        private readonly IConfigurationProvider<NotificationConfiguration> _notificationConfigurationService;
        private readonly IExceptionLogger _exceptionLogger;

        public NotificationsService(
            IEnumerable<INotifierService> notifiers,
            IConfigurationProvider<NotificationConfiguration> notificationConfigurationService,
            IExceptionLogger exceptionLogger)
        {
            _notifiers = notifiers;
            _notificationConfigurationService = notificationConfigurationService;
            _exceptionLogger = exceptionLogger;
        }

        public void ProcessNotification(NotifierData data)
        {
            var notifiers = GetNotifiers(data.NotificationType);

            if (!notifiers.Any())
            {
                _exceptionLogger.Log(new MissingNotificationException(data.NotificationType));
            }

            foreach (var notifier in notifiers)
            {
                try
                {
                    notifier.Notify(data);
                }
                catch (Exception ex)
                {
                    _exceptionLogger.Log(ex);
                }
            }
        }

        private IEnumerable<INotifierService> GetNotifiers(NotificationTypeEnum notificationType)
        {
            var notifierTypes = GetNotifierTypes(notificationType);
            var configuration = _notificationConfigurationService.GetSettings();

            foreach (var notifierType in notifierTypes)
            {
                var notifierConfiguration = configuration.NotifierConfigurations.Single(n => n.NotifierType == notifierType);
                if (!notifierConfiguration.Enabled)
                {
                    continue;
                }

                var notifier = _notifiers.SingleOrDefault(n => n.Type == notifierType);
                if (notifier == null)
                {
                    _exceptionLogger.Log(new MissingNotifierException(notifierType, notificationType));
                }

                yield return notifier;
            }
        }

        private IEnumerable<NotifierTypeEnum> GetNotifierTypes(NotificationTypeEnum notificationType)
        {
            var configuration = _notificationConfigurationService.GetSettings();
            var notificationTypeConfiguration = configuration.NotificationTypeConfigurations.SingleOrDefault(c => c.NotificationType == notificationType);

            if (notificationTypeConfiguration == null || !notificationTypeConfiguration.NotifierTypes.IsEmpty())
            {
                return configuration.DefaultNotifier.ToEnumerableOfOne();
            }

            return notificationTypeConfiguration.NotifierTypes;
        }
    }
}