using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Core.Activity;
using uIntra.Core.Configuration;
using uIntra.Core.Exceptions;
using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using uIntra.Core.User;
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
        private readonly INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> _uiNotificationModelMapper;
        private readonly INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> _emailNotificationModelMapper;
        private readonly IUiNotifierService _uiNotifierService;
        private readonly IMailService _mailService;
        private readonly IIntranetUserService<IIntranetUser> _intranetUserService;

        public NotificationsService(
            IExceptionLogger exceptionLogger,
            IMemberNotifiersSettingsService memberNotifiersSettingsService,
            INotificationSettingsService notificationSettingsService,
            INotifierTypeProvider notifierTypeProvider,
            INotificationModelMapper<UiNotifierTemplate, UiNotificationMessage> uiNotificationModelMapper,
            IUiNotifierService uiNotifierService,
            IIntranetUserService<IIntranetUser> intranetUserService,
            INotificationModelMapper<EmailNotifierTemplate, EmailNotificationMessage> emailNotificationModelMapper,
            IMailService mailService)
        {
            _exceptionLogger = exceptionLogger;
            _memberNotifiersSettingsService = memberNotifiersSettingsService;
            _notificationSettingsService = notificationSettingsService;
            _notifierTypeProvider = notifierTypeProvider;
            _uiNotificationModelMapper = uiNotificationModelMapper;
            _uiNotifierService = uiNotifierService;
            _intranetUserService = intranetUserService;
            _emailNotificationModelMapper = emailNotificationModelMapper;
            _mailService = mailService;
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
                    var receivers = _intranetUserService.GetMany(receiverIds);
                    var messages = receivers.Select(r => _uiNotificationModelMapper.Map(data.Value, notifierSettings.Template, r));
                    try
                    {
                        _uiNotifierService.Notify(messages);
                    }
                    catch (Exception ex)
                    {
                        _exceptionLogger.Log(ex);
                    }
                }
            }

            (receiverIds, isNotEmpty) = GetReceiverIdsForNotifier(NotifierTypeEnum.EmailNotifier);

            if (isNotEmpty)
            {
                var notifierSettings = _notificationSettingsService.GetEmailNotifierSettings(
                    eventIdentity.AddNotifierIdentity(_notifierTypeProvider.Get((int) NotifierTypeEnum.EmailNotifier)));
                if (notifierSettings.IsEnabled)
                {
                    var receivers = _intranetUserService.GetMany(receiverIds);
                    var messages = receivers.Select(r => _emailNotificationModelMapper.Map(data.Value, notifierSettings.Template, r)).ToList();
                    try
                    {
                        messages.ForEach(_mailService.Send);
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