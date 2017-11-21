using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Configuration;
using uIntra.Core.Exceptions;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core;
using uIntra.Notification.Core.Services;
using uIntra.Notification.Exceptions;

namespace uIntra.Notification
{
    public class NotificationsService : INotificationsService
    {
        private readonly IEnumerable<INotifierService> _notifiers;
        private readonly IConfigurationProvider<NotificationConfiguration> _notificationConfigurationService;
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly INotifierTypeProvider _notifierTypeProvider;
        private readonly IActivityTypeProvider _activityTypeProvider;

        public NotificationsService(
            IEnumerable<INotifierService> notifiers,
            IConfigurationProvider<NotificationConfiguration> notificationConfigurationService,
            IExceptionLogger exceptionLogger,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            INotificationSettingsService notificationSettingsService,
            INotifierTypeProvider notifierTypeProvider, IActivityTypeProvider activityTypeProvider)
        {
            _notifiers = notifiers;
            _notificationConfigurationService = notificationConfigurationService;
            _exceptionLogger = exceptionLogger;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _notificationSettingsService = notificationSettingsService;
            _notifierTypeProvider = notifierTypeProvider;
            _activityTypeProvider = activityTypeProvider;
        }
         
        public void ProcessNotification(NotifierData data)
        {
            var notifiers = GetNotifiers(data.NotificationType).ToList();
            var allReceiversIds = data.ReceiverIds.ToList();
            var allReceiversNotifiersSettings = _memberNotifiersSettingsService.GetForMembers(allReceiversIds);



            if (!notifiers.Any())
            {
                _exceptionLogger.Log(new MissingNotificationException(data.NotificationType));
            }

            foreach (var notifier in notifiers)
            {

                if (data.Value is LikesNotifierDataModel model && notifier.Type == NotifierTypeEnum.UiNotifier)
                {
                    var settings = _notificationSettingsService.GetUiNotifierSettings(new ActivityEventNotifierIdentity(
                        new ActivityEventIdentity(_activityTypeProvider.Get((int)IntranetActivityTypeEnum.News), data.NotificationType),
                        _notifierTypeProvider.Get((int) NotifierTypeEnum.UiNotifier)));
                    if (!settings.IsEnabled) break;
                    model.Title = settings.Template.Message;
                    data.Value = model;
                }


                var receiverIds = allReceiversIds
                    .Where(receiverId => allReceiversNotifiersSettings[receiverId].Contains(notifier.Type))
                    .ToList();

                if (receiverIds.Count == 0 )
                {
                    continue;
                }

                data.ReceiverIds = receiverIds;

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

        private IEnumerable<INotifierService> GetNotifiers(IIntranetType notificationType)
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
                    continue;
                }

                yield return notifier;
            }
        }

        private IEnumerable<NotifierTypeEnum> GetNotifierTypes(IIntranetType notificationType)
        {
            var configuration = _notificationConfigurationService.GetSettings();
            var notificationTypeConfiguration = configuration.NotificationTypeConfigurations.SingleOrDefault(c => c.NotificationType == notificationType.Name);

            if (notificationTypeConfiguration == null || notificationTypeConfiguration.NotifierTypes.IsEmpty())
            {
                return configuration.DefaultNotifier.ToEnumerableOfOne();
            }

            return notificationTypeConfiguration.NotifierTypes;
        }
    }
}