using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Services;

namespace Uintra.Bulletins
{
    public class BulletinsSection
    {
        protected const string Alias = "bulletins";
        protected const string Name = "Bulletins";
        protected const string Icon = "icon-notepad color-red";

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