using System.Web.Mvc;
using Umbraco.Core.Services;

namespace Uintra.Notification.Dashboard
{
    public class NotificationSettingsSection
    {
        protected const string Alias = "NotificationSettings";
        protected const string Name = "Notifications";
        protected const string Icon = "icon-timer color-red";

        public static void AddSectionToAllUsers()
        {
            var sectionService = DependencyResolver.Current.GetService<ISectionService>();
            var section = sectionService.GetByAlias(Alias);
            if (section == null)
            {
                sectionService.MakeNew(Name, Alias, Icon, 1);
            }

            var userService = DependencyResolver.Current.GetService<IUserService>();
            var userGroups = userService.GetAllUserGroups();
            foreach (var userGroup in userGroups)
            {
                userGroup.AddAllowedSection(Alias);
                userService.Save(userGroup);
            }
        }
    }
}