using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Services;

namespace Uintra.Core.Helpers
{
    public class SectionHelper
    {
        public static void AddSectionToAllUsers(string name, string alias, string icon)
        {
            var sectionService = DependencyResolver.Current.GetService<ISectionService>();
            var userService = DependencyResolver.Current.GetService<IUserService>();

            var section = sectionService.GetByAlias(alias);
            if (section == null)
            {
                sectionService.MakeNew(name, alias, icon);
            }

            var userGroups = userService.GetAllUserGroups().Where(group => !group.AllowedSections.Contains(alias));

            foreach (var userGroup in userGroups)
            {
                userGroup.AddAllowedSection(alias);
                userService.Save(userGroup, raiseEvents: false);
            }
        }
    }
}
