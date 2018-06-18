using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uIntra.Notification;
using Uintra.Notification;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._0._0.Steps
{
    public class WelcomeMessageSetupStep : IMigrationStep
    {
        private readonly NotificationSettingsService _notificationSettingsService;

        public WelcomeMessageSetupStep(NotificationSettingsService notificationSettingsService)
        {
            _notificationSettingsService = notificationSettingsService;
        }

        public ExecutionResult Execute()
        {
            var settings = _notificationSettingsService.Get<PopupNotifierTemplate>(
                new ActivityEventNotifierIdentity(
                    CommunicationTypeEnum.Member,
                    NotificationTypeEnum.Welcome,
                    NotifierTypeEnum.PopupNotifier));

            settings.IsEnabled = false;

            _notificationSettingsService.Save(settings);
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            var settings = _notificationSettingsService.Get<PopupNotifierTemplate>(
                new ActivityEventNotifierIdentity(
                    CommunicationTypeEnum.Member,
                    NotificationTypeEnum.Welcome,
                    NotifierTypeEnum.PopupNotifier));

            settings.IsEnabled = true;

            _notificationSettingsService.Save(settings);
        }
    }
}