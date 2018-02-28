using Umbraco.Core;

namespace Uintra.Notification.Dashboard
{
    public class NotificationSettingsSection
    {
        protected const string Alias = "NotificationSettings";
        protected const string Name = "Notifications";
        protected const string Icon = "icon-timer color-red";

        public static void AddSectionToAllUsers(ApplicationContext applicationContext)
        {
            var section = applicationContext.Services.SectionService.GetByAlias(Alias);
            if (section == null)
            {
                applicationContext.Services.SectionService.MakeNew(Name, Alias, Icon, 1);
            }

            var userService = applicationContext.Services.UserService;
            var userGroups = userService.GetAllUserGroups();
            foreach (var userGroup in userGroups)
            {
                userGroup.AddAllowedSection(Alias);
                userService.Save(userGroup);
            }
        }
    }
}