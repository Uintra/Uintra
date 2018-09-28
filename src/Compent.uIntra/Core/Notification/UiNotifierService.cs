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
        private readonly NotificationSettingsService _notificationSettingsService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;
        private readonly UiNotificationService _notificationsService;

        public Enum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(
            INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper,
            NotificationSettingsService notificationSettingsService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            UiNotificationService notificationsService)
        {
            _notificationModelMapper = notificationModelMapper;
            _notificationSettingsService = notificationSettingsService;
            _intranetUserService = intranetUserService;
            _notificationsService = notificationsService;
        }

        public void Notify(NotifierData data)
        {
            var isCommunicationSettings = data.NotificationType.In(
                NotificationTypeEnum.CommentLikeAdded,
                NotificationTypeEnum.MonthlyMail,
                NotificationTypeEnum.UserMention,
                IntranetActivityTypeEnum.ContentPage,
                IntranetActivityTypeEnum.PagePromotion);

            var identity = new ActivityEventIdentity(isCommunicationSettings
                    ? CommunicationTypeEnum.CommunicationSettings
                    : data.ActivityType, data.NotificationType)
                .AddNotifierIdentity(Type);

            var settings = _notificationSettingsService.Get<UiNotifierTemplate>(identity);
            if (settings == null || !settings.IsEnabled) return;
            var receivers = _intranetUserService.GetMany(data.ReceiverIds);

            var messages = receivers.Select(r => _notificationModelMapper.Map(data.Value, settings.Template, r));
            _notificationsService.Notify(messages);
        }
    }

}