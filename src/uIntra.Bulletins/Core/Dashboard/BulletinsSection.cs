using Umbraco.Core;

namespace Uintra.Bulletins
{
    public class BulletinsSection
    {
        protected const string Alias = "bulletins";
        protected const string Name = "Bulletins";
        protected const string Icon = "icon-notepad color-red";

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