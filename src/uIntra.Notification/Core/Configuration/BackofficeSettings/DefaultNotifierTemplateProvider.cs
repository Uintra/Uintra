using uIntra.Core.Extensions;
using uIntra.Core.TypeProviders;
using uIntra.Notification.Core;

namespace uIntra.Notification.Configuration
{
    public class 
        BackofficeNotificationSettingsProvider :
        IBackofficeNotificationSettingsProvider<EmailNotifierTemplate>,
        IBackofficeNotificationSettingsProvider<UiNotifierTemplate>
    {
        private readonly IBackofficeSettingsReader _backofficeSettingsReader;
        private readonly INotifierTypeProvider _notifierTypeProvider;

        public BackofficeNotificationSettingsProvider(IBackofficeSettingsReader backofficeSettingsReader, INotifierTypeProvider notifierTypeProvider)
        {
            _backofficeSettingsReader = backofficeSettingsReader;
            _notifierTypeProvider = notifierTypeProvider;
        }

        NotificationSettingDefaults<EmailNotifierTemplate> IBackofficeNotificationSettingsProvider<EmailNotifierTemplate>.GetSettings(ActivityEventIdentity activityEvent)
        {
            return GetTemplate<EmailNotifierTemplate>(activityEvent, _notifierTypeProvider.Get((int)NotifierTypeEnum.EmailNotifier));
        }

        NotificationSettingDefaults<UiNotifierTemplate> IBackofficeNotificationSettingsProvider<UiNotifierTemplate>.GetSettings(ActivityEventIdentity activityEvent)
        {
            return GetTemplate<UiNotifierTemplate>(activityEvent, _notifierTypeProvider.Get((int)NotifierTypeEnum.UiNotifier));
        }

        private NotificationSettingDefaults<T> GetTemplate<T>(ActivityEventIdentity activityEvent, IIntranetType notifier)
            where T : INotifierTemplate
        {
            var notificationType = activityEvent.AddNotifierIdentity(notifier);
            var result = _backofficeSettingsReader.ReadSettings(notificationType).Deserialize<NotificationSettingDefaults<T>>();
            return result;
        }
    }
}