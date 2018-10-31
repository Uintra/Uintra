using Uintra.Core.Extensions;
using Uintra.Core.Persistence;
using Uintra.Notification;
using Uintra.Notification.Configuration;
using Uintra.Notification.Constants;

namespace Compent.Uintra.Core.Updater.Migrations._1._0.Steps
{
    public class UpdateUiNotificationSettingsStep : IMigrationStep
    {
        private readonly INotificationSettingsService _notificationSettingsService;
        private readonly ISqlRepository<NotificationSetting> _repository;

        public UpdateUiNotificationSettingsStep(INotificationSettingsService notificationSettingsService,
            ISqlRepository<NotificationSetting> repository)
        {
            _notificationSettingsService = notificationSettingsService;
            _repository = repository;
        }

        public ExecutionResult Execute()
        {
            var uiSettings = _repository.FindAll(i => i.NotifierType == (int)NotifierTypeEnum.UiNotifier);
            foreach (var settings in uiSettings)
            {
                var value = settings.JsonData.Deserialize<UiNotifierTemplate>();
                value.DesktopTitle = TokensConstants.NotificationType;
                value.DesktopMessage = value.Message?.StripHtml();
                value.IsDesktopNotificationEnabled = false;
                settings.JsonData = value.ToJson();
            }
            _repository.Update(uiSettings);
            return ExecutionResult.Success;
        }

        public void Undo()
        {
            
        }
    }
}