using Uintra.Features.Notification.Configuration;

namespace Uintra.Core.Member.Profile.Edit.Models
{
    public class ProfileEditNotificationSettings
    {
        public bool IsEnabled { get; set; }
        public NotifierTypeEnum NotifierTypeEnum { get; set; }
    }
}