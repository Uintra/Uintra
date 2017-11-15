using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uIntra.Core.Activity;
using uIntra.Notification.Configuration;

namespace uIntra.Notification.Core.Models
{
    public interface INotifierSettingModel
    {
        bool IsEnabled { get; set; }
    }

    public class EmailNotifierSettingModel : INotifierSettingModel
    {
        public bool IsEnabled { get; set; }
    }

    public class EmailNotifierTemplate
    {
        private string Subject { get; set; }
        private string Content { get; set; }
    }

    public class UiNotifierSettingModel : INotifierSettingModel
    {
        public bool IsEnabled { get; set; }
    }

    public class NotifierSettingsModel
    {
        public IntranetActivityTypeEnum ActivityType { get; set; }
        public NotificationTypeEnum NotificationType { get; set; }
        public EmailNotifierSettingModel EmailNotifierSetting { get; set; }
        public UiNotifierSettingModel UiNotifierSetting { get; set; }
    }
}