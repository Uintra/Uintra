using Uintra.Core.Activity;
using Uintra.Notification;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Updater.Migrations._0._5.Steps
{
    public class DisableMemberNotificationStep : IMigrationStep
    {
        private readonly INotificationSettingsService _notificationSettingsService;

        public DisableMemberNotificationStep(INotificationSettingsService notificationSettingsService)
        {
            _notificationSettingsService = notificationSettingsService;
        }

        public ExecutionResult Execute()
        {
            UpdateDefaultNotificationSettings();
            return ExecutionResult.Success;
        }

        private void UpdateDefaultNotificationSettings()
        {
            var popupSettings = GetSettings();
            popupSettings.IsEnabled = false;
            _notificationSettingsService.Save(popupSettings);

            var emailSettings = GetEmailSettings();
            emailSettings.IsEnabled = false;
            _notificationSettingsService.Save(emailSettings);
        }

        private void ReverDefaultNotificationSettings()
        {
            var popupSettings = GetSettings();
            popupSettings.IsEnabled = true;
            _notificationSettingsService.Save(popupSettings);
            var emailSettings = GetEmailSettings();
            emailSettings.IsEnabled = true;
            _notificationSettingsService.Save(emailSettings);
        }

        private NotifierSettingModel<PopupNotifierTemplate> GetSettings()
        {
            return _notificationSettingsService.Get<PopupNotifierTemplate>(
                new ActivityEventNotifierIdentity(
                    new ActivityEventIdentity(CommunicationTypeEnum.Member, NotificationTypeEnum.Welcome),
                    NotifierTypeEnum.PopupNotifier));
        }

        private NotifierSettingModel<EmailNotifierTemplate> GetEmailSettings()
        {
            return _notificationSettingsService.Get<EmailNotifierTemplate>(
                new ActivityEventNotifierIdentity(
                    new ActivityEventIdentity(IntranetActivityTypeEnum.Events, NotificationTypeEnum.BeforeStart),
                    NotifierTypeEnum.EmailNotifier));
        }

        public void Undo()
        {
            ReverDefaultNotificationSettings();
        }
    }
}
