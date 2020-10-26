using Uintra.Features.Notification.Models.NotifierTemplates;

namespace Uintra.Features.Notification.Models.Configuration
{
    public class NotifierSettingsModel
    {
        public NotifierSettingModel<EmailNotifierTemplate> EmailNotifierSetting { get; set; }
        public NotifierSettingModel<UiNotifierTemplate> UiNotifierSetting { get; set; }
        public NotifierSettingModel<PopupNotifierTemplate> PopupNotifierSetting { get; set; }
        public NotifierSettingModel<DesktopNotifierTemplate> DesktopNotifierSetting { get; set; }
    }
}