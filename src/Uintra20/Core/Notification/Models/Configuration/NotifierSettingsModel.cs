﻿namespace Uintra20.Core.Notification
{
    public class NotifierSettingsModel
    {
        public NotifierSettingModel<EmailNotifierTemplate> EmailNotifierSetting { get; set; }
        public NotifierSettingModel<UiNotifierTemplate> UiNotifierSetting { get; set; }
        public NotifierSettingModel<PopupNotifierTemplate> PopupNotifierSetting { get; set; }
        public NotifierSettingModel<DesktopNotifierTemplate> DesktopNotifierSetting { get; set; }
    }
}