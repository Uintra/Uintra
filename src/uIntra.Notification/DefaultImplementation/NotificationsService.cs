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
        private readonly IExceptionLogger _exceptionLogger;
        private readonly IMemberNotifiersSettingsService _memberNotifiersSettingsService;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly INotifierTypeProvider _notifierTypeProvider;
        private readonly INotificationModelMapper<UiNotificationMessage> _uiNotificationModelMapper;
        private readonly IUiNotifierService _uiNotifierService;

        public NotificationsService(
            IExceptionLogger exceptionLogger,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            INotificationSettingsService notificationSettingsService,
            INotifierTypeProvider notifierTypeProvider,
            INotificationModelMapper<UiNotificationMessage> uiNotificationModelMapper,
            IUiNotifierService uiNotifierService)
        {
            _exceptionLogger = exceptionLogger;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _notificationSettingsService = notificationSettingsService;
            _notifierTypeProvider = notifierTypeProvider;
            _uiNotificationModelMapper = uiNotificationModelMapper;
            _uiNotifierService = uiNotifierService;
        }

        public void ProcessNotification(NotifierData data)
        {
            var allReceiversIds = data.ReceiverIds.ToList();
            var allReceiversNotifiersSettings = _memberNotifiersSettingsService.GetForMembers(allReceiversIds);

            (IEnumerable<Guid> receiverIds, bool isNotEmpty) GetReceiverIdsForNotifier(NotifierTypeEnum notifierType)
            {
                var ids = allReceiversIds
                    .Where(receiverId => allReceiversNotifiersSettings[receiverId].Contains(notifierType))
                    .ToList();
                return (ids, ids.Any());
            }

            var eventIdentity = new ActivityEventIdentity(data.ActivityType, data.NotificationType);

            var (receiverIds, isNotEmpty) = GetReceiverIdsForNotifier(_uiNotifierService.Type);

            if (isNotEmpty)
            {
                var notifierSettings = _notificationSettingsService.GetUiNotifierSettings(
                    eventIdentity.AddNotifierIdentity(_notifierTypeProvider.Get((int) NotifierTypeEnum.UiNotifier)));
                if (notifierSettings.IsEnabled)
                {
                    data.ReceiverIds = receiverIds;
                    var message = _uiNotificationModelMapper.Map(data, notifierSettings.Template);
                    try
                    {
                        _uiNotifierService.Notify(message);
                    }
                    catch (Exception ex)
                    {
                        _exceptionLogger.Log(ex);
                    }
                }
            }
        }
    }
}