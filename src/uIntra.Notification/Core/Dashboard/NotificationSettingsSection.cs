using Uintra.Core.Helpers;

namespace Uintra.Notification.Dashboard
{
    public class NotificationSettingsSection
    {
        protected const string Alias = "NotificationSettings";
        protected const string Name = "Notifications";
        protected const string Icon = "icon-timer color-red";

        public static void AddSectionToAllUsers()
        {
            SectionHelper.AddSectionToAllUsers(Name, Alias, Icon);
        }
    }
}