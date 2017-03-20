using System;
using System.Collections.Generic;
using System.Linq;
//using Elmah;
using uCommunity.Notification.Exceptions;
using uCommunity.Core.Configuration;

namespace uCommunity.Notification.Notifier
{
    public class NotificationService : INotificationService
    {
        private readonly IEnumerable<INotifierService> _notifiers;
        private readonly IConfigurationProvider<NotificationConfiguration> _notificationConfigurationService;

        public NotificationService(
            IEnumerable<INotifierService> notifiers,
            IConfigurationProvider<NotificationConfiguration> notificationConfigurationService)
        {
            _notifiers = notifiers;
            _notificationConfigurationService = notificationConfigurationService;
        }

        public void ProcessNotification(NotifierData data)
        {
            var notifiers = GetNotifiers(data.NotificationType).ToList();

            if (notifiers.Count == 0)
            {
                //ErrorSignal.FromCurrentContext().Raise(new MissingNotificationException(data.NotificationType));
            }

            foreach (var notifier in notifiers)
            {
                try
                {
                    notifier.Notify(data);
                }
                catch (Exception ex)
                {
                    //ErrorSignal.FromCurrentContext().Raise(ex);
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
                    //ErrorSignal.FromCurrentContext().Raise(new MissingNotifierException(notifierType, notificationType));
                }

                yield return notifier;
            }
        }

        private IEnumerable<NotifierTypeEnum> GetNotifierTypes(NotificationTypeEnum notificationType)
        {
            var configuration = _notificationConfigurationService.GetSettings();
            var notificationTypeConfiguration = configuration.NotificationTypeConfigurations.SingleOrDefault(c => c.NotificationType == notificationType);

            if (notificationTypeConfiguration == null || !notificationTypeConfiguration.NotifierTypes.Any())
            {
                return Enumerable.Repeat(configuration.DefaultNotifier, 1);
                    //configuration.DefaultNotifier.ToEnumerableOfOne();
            }

            return notificationTypeConfiguration.NotifierTypes;
        }
    }
}