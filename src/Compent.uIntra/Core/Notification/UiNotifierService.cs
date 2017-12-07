using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Extensions;
using uIntra.Core.Persistence;
using uIntra.Core.User;
using uIntra.Notification;
using uIntra.Notification.Base;
using uIntra.Notification.Configuration;
using uIntra.Notification.Core.Services;
using uIntra.Notification.DefaultImplementation;

namespace Compent.uIntra.Core.Notification
{
    public class UiNotifierService : INotifierService
    {
       
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _notificationModelMapper;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly NotifierTypeProvider _notifierTypeProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly UiNotificationService _notificationsService;

        public NotifierTypeEnum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(
            INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper,
            NotificationSettingsService notificationSettingsService,
            NotifierTypeProvider notifierTypeProvider,
            IIntranetUserService<IIntranetUser> intranetUserService, UiNotificationService notificationsService)
        {
            _notificationModelMapper = notificationModelMapper;
            _notificationSettingsService = notificationSettingsService;
            _notifierTypeProvider = notifierTypeProvider;
            _intranetUserService = intranetUserService;
            _notificationsService = notificationsService;
        }

        public void Notify(NotifierData data)
        {
            var identity = new ActivityEventIdentity(data.ActivityType, data.NotificationType)
                .AddNotifierIdentity(_notifierTypeProvider.Get((int)Type));

            var settings = _notificationSettingsService.Get<UiNotifierTemplate>(identity);
            if (!settings.IsEnabled) return;
            var receivers = _intranetUserService.GetMany(data.ReceiverIds);

            var messages = receivers.Select(r => _notificationModelMapper.Map(data.Value, settings.Template, r));
            _notificationsService.Notify(messages);
        }
    }

}