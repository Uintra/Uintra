using System;
using System.Linq;
using Compent.Extensions;
using Uintra.Core.Activity;
using Uintra.Core.User;
using Uintra.Notification;
using Uintra.Notification.Base;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Notification
{
    public class UiNotifierService : INotifierService
    {
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _notificationModelMapper;
        private readonly INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage> _desktopNotificationModelMapper;
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly UiNotificationService _notificationsService;

        public Enum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(
            INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper,
            INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage> desktopNotificationModelMapper,
            NotificationSettingsService notificationSettingsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            UiNotificationService notificationsService)
        {
            _notificationModelMapper = notificationModelMapper;
            _notificationSettingsService = notificationSettingsService;
            _intranetUserService = intranetUserService;
            _notificationsService = notificationsService;
            _desktopNotificationModelMapper = desktopNotificationModelMapper;
        }

        public void Notify(NotifierData data)
        {
            var isCommunicationSettings = data.NotificationType.In(
                NotificationTypeEnum.CommentLikeAdded,
                NotificationTypeEnum.MonthlyMail,
                IntranetActivityTypeEnum.ContentPage,
                IntranetActivityTypeEnum.PagePromotion);

            var identity = new ActivityEventIdentity(isCommunicationSettings
                    ? CommunicationTypeEnum.CommunicationSettings
                    : data.ActivityType, data.NotificationType)
                .AddNotifierIdentity(Type);
            var settings = _notificationSettingsService.Get<UiNotifierTemplate>(identity);

            //if (settings == null) return;

            var desktopSettingsIdentity = new ActivityEventIdentity(settings.ActivityType, settings.NotificationType)
                    .AddNotifierIdentity(NotifierTypeEnum.DesktopNotifier);
            var desktopSettings = _notificationSettingsService.Get<DesktopNotifierTemplate>(desktopSettingsIdentity);

            if (!settings.IsEnabled && desktopSettings.IsEnabled) return;

            var receivers = _intranetUserService.GetMany(data.ReceiverIds);

            var messages = receivers.Select(receiver =>
            {
                var uiMsg = _notificationModelMapper.Map(data.Value, settings.Template, receiver);
                if (desktopSettings.IsEnabled)
                {
                    var desktopMsg = _desktopNotificationModelMapper.Map(data.Value, desktopSettings.Template, receiver);
                    uiMsg.DesktopTitle = desktopMsg.Title;
                    uiMsg.DesktopMessage = desktopMsg.Message;
                    uiMsg.IsDesktopNotificationEnabled = true;
                }
                return uiMsg;
            });
            _notificationsService.Notify(messages);
        }
    }
}