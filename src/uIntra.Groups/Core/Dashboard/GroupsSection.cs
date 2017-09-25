using Umbraco.Core;

namespace uIntra.Groups.Dashboard
{
    public class GroupsSection
    {
        protected const string Alias = "groups";
        protected const string Name = "Groups";
        protected const string Icon = "icon-chat _groups-section-color";

        public static void AddSectionToAllUsers(ApplicationContext applicationContext)
        {
            var section = applicationContext.Services.SectionService.GetByAlias(Alias);
            if (section != null) return;

            applicationContext.Services.SectionService.MakeNew(Name, Alias, Icon, 1);
            applicationContext.Services.UserService.AddSectionToAllUsers(Alias);
        }
    }
}