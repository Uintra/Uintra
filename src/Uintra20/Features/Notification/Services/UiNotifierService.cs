using System;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
using LanguageExt;
using Uintra20.Core.Activity;
using Uintra20.Core.Member;
using Uintra20.Features.Notification.Configuration;
using Uintra20.Features.Notification.Entities.Base;
using Uintra20.Features.Notification.Models;
using Uintra20.Features.Notification.Models.NotifierTemplates;
using Uintra20.Infrastructure.Extensions;

namespace Uintra20.Features.Notification.Services
{
    public class UiNotifierService : INotifierService
    {
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _notificationModelMapper;
        private readonly INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage> _desktopNotificationModelMapper;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IIntranetMemberService<IIntranetMember> _intranetMemberService;
        private readonly UiNotificationService _notificationsService;

        public Enum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(
            INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper,
            INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage> desktopNotificationModelMapper,
            INotificationSettingsService notificationSettingsService,
            IIntranetMemberService<IIntranetMember> intranetMemberService,
            UiNotificationService notificationsService)
        {
            _notificationModelMapper = notificationModelMapper;
            _notificationSettingsService = notificationSettingsService;
            _intranetMemberService = intranetMemberService;
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

            if (!settings.IsEnabled && !desktopSettings.IsEnabled) return;

            var receivers = _intranetMemberService.GetMany(data.ReceiverIds).ToList();

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

        public async Task NotifyAsync(NotifierData data)
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
            var settings = await _notificationSettingsService.GetAsync<UiNotifierTemplate>(identity);

            //if (settings == null) return;

            var desktopSettingsIdentity = new ActivityEventIdentity(settings.ActivityType, settings.NotificationType)
                .AddNotifierIdentity(NotifierTypeEnum.DesktopNotifier);
            var desktopSettings = await _notificationSettingsService.GetAsync<DesktopNotifierTemplate>(desktopSettingsIdentity);

            if (!settings.IsEnabled && !desktopSettings.IsEnabled) return;

            //var receivers = await _intranetMemberService.GetManyAsync(data.ReceiverIds).Select(x => x.ToList());
            var receivers = _intranetMemberService.GetMany(data.ReceiverIds).ToList();

            //var messages = await receivers.SelectAsync(async receiver =>
            var messages = receivers.Select(receiver =>
            {
                //var uiMsg = await _notificationModelMapper.MapAsync(data.Value, settings.Template, receiver);
                var uiMsg = _notificationModelMapper.Map(data.Value, settings.Template, receiver);
                if (desktopSettings.IsEnabled)
                {
                    //var desktopMsg = await _desktopNotificationModelMapper.MapAsync(data.Value, desktopSettings.Template, receiver);
                    var desktopMsg = _desktopNotificationModelMapper.Map(data.Value, desktopSettings.Template, receiver);
                    uiMsg.DesktopTitle = desktopMsg.Title;
                    uiMsg.DesktopMessage = desktopMsg.Message;
                    uiMsg.IsDesktopNotificationEnabled = true;
                }
                return uiMsg;
            });
            await _notificationsService.NotifyAsync(messages);
        }
    }
}