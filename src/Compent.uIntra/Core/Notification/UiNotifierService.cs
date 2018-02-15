using System;
using System.Linq;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;
using Uintra.Notification.DefaultImplementation;

namespace Compent.Uintra.Core.Notification
{
    public class UiNotifierService : INotifierService
    {
       
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _notificationModelMapper;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly NotifierTypeProvider _notifierTypeProvider;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly UiNotificationService _notificationsService;

        public Enum Type => NotifierTypeEnum.UiNotifier;

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
            var identity = new ActivityEventIdentity(data.ActivityType, data.NotificationType).AddNotifierIdentity(Type);

            var settings = _notificationSettingsService.Get<UiNotifierTemplate>(identity);
            if (!settings.IsEnabled) return;
            var receivers = _intranetUserService.GetMany(data.ReceiverIds);

            var messages = receivers.Select(r => _notificationModelMapper.Map(data.Value, settings.Template, r));
            _notificationsService.Notify(messages);
        }
    }

}