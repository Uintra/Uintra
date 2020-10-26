using Uintra20.Features.Notification.Configuration;

namespace Uintra20.Core.Member.Profile.Edit.Models
{
    public class ProfileEditNotificationSettings
    {
        public bool IsEnabled { get; set; }
        public NotifierTypeEnum NotifierTypeEnum { get; set; }
    }
}