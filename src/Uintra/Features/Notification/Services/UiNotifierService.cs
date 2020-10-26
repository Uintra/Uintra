﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Compent.Extensions;
using Uintra.Core.Activity;
using Uintra.Core.Member.Entities;
using Uintra.Core.Member.Services;
using Uintra.Features.Notification.Configuration;
using Uintra.Features.Notification.Entities.Base;
using Uintra.Features.Notification.Models;
using Uintra.Features.Notification.Models.NotifierTemplates;

namespace Uintra.Features.Notification.Services
{
    public class UiNotifierService : INotifierService
    {
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _notificationModelMapper;
        private readonly INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage> _desktopNotificationModelMapper;
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly IIntranetMemberService<IntranetMember> _intranetMemberService;
        private readonly IUiNotificationService _notificationsService;

        public Enum Type => NotifierTypeEnum.UiNotifier;

        public UiNotifierService(
            INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> notificationModelMapper,
            INotificationModelMapper<DesktopNotifierTemplate, DesktopNotificationMessage> desktopNotificationModelMapper,
            INotificationSettingsService notificationSettingsService,
            IIntranetMemberService<IntranetMember> intranetMemberService,
            IUiNotificationService notificationsService)
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
                IntranetActivityTypeEnum.ContentPage);

            var identity = new ActivityEventIdentity(isCommunicationSettings
                    ? CommunicationTypeEnum.CommunicationSettings
                    : data.ActivityType, data.NotificationType)
                .AddNotifierIdentity(Type);
            var settings = _notificationSettingsService.Get<UiNotifierTemplate>(identity);

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
                    if (uiMsg.NotifierId.HasValue)
                    {
                        uiMsg.NotifierPhotoUrl = _intranetMemberService.Get(uiMsg.NotifierId.Value)?.Photo;    
                    }
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
                IntranetActivityTypeEnum.ContentPage);

            var identity = new ActivityEventIdentity(isCommunicationSettings
                    ? CommunicationTypeEnum.CommunicationSettings
                    : data.ActivityType, data.NotificationType)
                .AddNotifierIdentity(Type);
            var settings = await _notificationSettingsService.GetAsync<UiNotifierTemplate>(identity);

            var desktopSettingsIdentity = new ActivityEventIdentity(settings.ActivityType, settings.NotificationType)
                .AddNotifierIdentity(NotifierTypeEnum.DesktopNotifier);
            var desktopSettings = await _notificationSettingsService.GetAsync<DesktopNotifierTemplate>(desktopSettingsIdentity);

            if (!settings.IsEnabled && !desktopSettings.IsEnabled) return;

            var receivers = (await _intranetMemberService.GetManyAsync(data.ReceiverIds));
            
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
            await _notificationsService.NotifyAsync(messages);
        }
    }
}