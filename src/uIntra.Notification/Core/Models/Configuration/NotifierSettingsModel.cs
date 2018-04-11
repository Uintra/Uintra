namespace Uintra.Notification
{
    public class NotifierSettingsModel
    {
        public NotifierSettingModel<EmailNotifierTemplate> EmailNotifierSetting { get; set; }
        public NotifierSettingModel<UiNotifierTemplate> UiNotifierSetting { get; set; }
        public NotifierSettingModel<PopupNotifierTemplate> PopupNotifierSetting { get; set; }
    }
}