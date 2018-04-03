using System;
using System.Linq;
using uIntra.Notification;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Notification
{
    public class PopupNotifierService : INotifierService
    {
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage> _notificationModelMapper;
        private readonly IPopupNotificationService _notificationsService;
        public Enum Type => NotifierTypeEnum.PopupNotifier;

        public PopupNotifierService(
            INotificationSettingsService notificationSettingsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationModelMapper<PopupNotifierTemplate, PopupNotificationMessage> notificationModelMapper,
        IPopupNotificationService notificationsService
            )
        {
            _notificationSettingsService = notificationSettingsService;
            _intranetUserService = intranetUserService;
            _notificationModelMapper = notificationModelMapper;
            _notificationsService = notificationsService;
        }

        public void Notify(NotifierData data)
        {
            var settings = new NotifierSettingModel<PopupNotifierTemplate>()
            {
                IsEnabled = true,
                NotificationInfo = "TEST TEST TEST",
                NotificationTypeName = NotificationTypeEnum.Welcome.ToString(),
                NotificationType = NotificationTypeEnum.Welcome,
                NotifierType = NotifierTypeEnum.PopupNotifier,
                Template = new PopupNotifierTemplate()
                {
                    Message = "TEST POPUP MESSAGE"
                }
            };

            if (!settings.IsEnabled) return;
            var receivers = _intranetUserService.GetMany(data.ReceiverIds);

            var messages = receivers.Select(r => _notificationModelMapper.Map(data.Value, settings.Template, r));
            _notificationsService.Notify(messages);
        }
    }
}