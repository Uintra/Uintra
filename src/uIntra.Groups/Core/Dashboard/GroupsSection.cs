using Uintra.Core.Helpers;

namespace Uintra.Groups.Dashboard
{
    public class GroupsSection
    {
        protected const string Alias = "groups";
        protected const string Name = "Groups";
        protected const string Icon = "icon-chat color-red";

        public static void AddSectionToAllUsers()
        {
            SectionHelper.AddSectionToAllUsers(Name, Alias, Icon);
        }
    }
}