using Uintra.Notification;
using Uintra.Notification.Configuration;

namespace Compent.Uintra.Core.Updater.Migrations._0._3._2._0.Steps
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
        }

        private void ReverDefaultNotificationSettings()
        {
            var popupSettings = GetSettings();
            popupSettings.IsEnabled = true;
            _notificationSettingsService.Save(popupSettings);
        }

        private NotifierSettingModel<PopupNotifierTemplate> GetSettings()
        {
            return _notificationSettingsService.Get<PopupNotifierTemplate>(
                new ActivityEventNotifierIdentity(
                    new ActivityEventIdentity(CommunicationTypeEnum.Member, NotificationTypeEnum.Welcome),
                    NotifierTypeEnum.PopupNotifier));
        }

        public void Undo()
        {
            ReverDefaultNotificationSettings();
        }
    }
}