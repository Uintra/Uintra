using uIntra.Core.Extensions;

namespace uIntra.Notification.Configuration
{
    public class BackofficeNotificationSettingsProvider :
        IBackofficeNotificationSettingsProvider<EmailNotifierTemplate>,
        IBackofficeNotificationSettingsProvider<UiNotifierTemplate>
    {
        private readonly IBackofficeSettingsReader _backofficeSettingsReader;

        public BackofficeNotificationSettingsProvider(IBackofficeSettingsReader backofficeSettingsReader)
        {
            _backofficeSettingsReader = backofficeSettingsReader;
        }

        BackofficeNotificationSettingsModel<EmailNotifierTemplate> IBackofficeNotificationSettingsProvider<EmailNotifierTemplate>.GetSettings(ActivityEventIdentity activityEvent)
        {
            return GetTemplate<EmailNotifierTemplate>(activityEvent, NotifierTypeEnum.EmailNotifier);
        }

        BackofficeNotificationSettingsModel<UiNotifierTemplate> IBackofficeNotificationSettingsProvider<UiNotifierTemplate>.GetSettings(ActivityEventIdentity activityEvent)
        {
            return GetTemplate<UiNotifierTemplate>(activityEvent, NotifierTypeEnum.UiNotifier);
        }

        private BackofficeNotificationSettingsModel<T> GetTemplate<T>(ActivityEventIdentity activityEvent, NotifierTypeEnum notifier)
            where T : INotifierTemplate
        {
            var notificationType = activityEvent.AddNotifierIdentity(notifier);
            var result = _backofficeSettingsReader.ReadSettings(notificationType).Deserialize<BackofficeNotificationSettingsModel<T>>();
            return result;
        }
    }
}