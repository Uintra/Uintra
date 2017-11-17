using uIntra.Core.Extensions;

namespace uIntra.Notification.Configuration
{
    public class BackofficeNotificationSettingsProvider :
        IBackofficeNotificationSettingsProvider<EmailNotifierTemplate>,
        IBackofficeNotificationSettingsProvider<UiNotifierTemplate>
    {
        private readonly IDefaultTemplateReader _defaultTemplateReader;

        public BackofficeNotificationSettingsProvider(IDefaultTemplateReader defaultTemplateReader)
        {
            _defaultTemplateReader = defaultTemplateReader;
        }

        BackofficeNotificationSettingsModel<EmailNotifierTemplate> IBackofficeNotificationSettingsProvider<EmailNotifierTemplate>.GetBackofficeSettings(ActivityEventIdentity activityEvent)
        {
            return GetTemplate<EmailNotifierTemplate>(activityEvent, NotifierTypeEnum.EmailNotifier);
        }

        BackofficeNotificationSettingsModel<UiNotifierTemplate> IBackofficeNotificationSettingsProvider<UiNotifierTemplate>.GetBackofficeSettings(ActivityEventIdentity activityEvent)
        {
            return GetTemplate<UiNotifierTemplate>(activityEvent, NotifierTypeEnum.UiNotifier);
        }

        private BackofficeNotificationSettingsModel<T> GetTemplate<T>(ActivityEventIdentity activityEvent, NotifierTypeEnum notifier)
            where T : INotifierTemplate
        {
            var notificationType = activityEvent.AddNotifierIdentity(notifier);
            var result = _defaultTemplateReader.ReadTemplate(notificationType).Deserialize<BackofficeNotificationSettingsModel<T>>();
            return result;
        }
    }
}