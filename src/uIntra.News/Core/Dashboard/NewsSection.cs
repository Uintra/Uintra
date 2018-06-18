using Umbraco.Core;

namespace Uintra.News.Dashboard
{
    public class NewsSection
    {
        protected const string Alias = "news";
        protected const string Name = "News";
        protected const string Icon = "icon-newspaper-alt color-red";

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