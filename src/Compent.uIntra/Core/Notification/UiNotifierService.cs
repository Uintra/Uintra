using System;
using System.Linq;
using Extensions;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Notification
{
    public class UiNotifierService : INotifierService
    {
       
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _notificationModelMapper;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly UiNotificationService _notificationsService;

        public Enum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(
            INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper,
            NotificationSettingsService notificationSettingsService,
            IIntranetUserService<IIntranetUser> intranetUserService, UiNotificationService notificationsService)
        {
            _notificationModelMapper = notificationModelMapper;
            _notificationSettingsService = notificationSettingsService;
            _intranetUserService = intranetUserService;
            _notificationsService = notificationsService;
        }

        public void Notify(NotifierData data)
        {
            if (data.NotificationType.In(NotificationTypeEnum.CommentLikeAdded)) //TODO: temporary for communication settings
            {
               return;
            }

            var identity = new ActivityEventIdentity( data.ActivityType, data.NotificationType).AddNotifierIdentity(Type);

            var settings = _notificationSettingsService.Get<UiNotifierTemplate>(identity);
            if (!settings.IsEnabled) return;
            var receivers = _intranetUserService.GetMany(data.ReceiverIds);

            var messages = receivers.Select(r => _notificationModelMapper.Map(data.Value, settings.Template, r));
            _notificationsService.Notify(messages);
        }
    }

}