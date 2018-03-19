using System.Web.Mvc;
using Umbraco.Core.Services;

namespace Uintra.Groups.Dashboard
{
    public class GroupsSection
    {
        protected const string Alias = "groups";
        protected const string Name = "Groups";
        protected const string Icon = "icon-chat color-red";

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