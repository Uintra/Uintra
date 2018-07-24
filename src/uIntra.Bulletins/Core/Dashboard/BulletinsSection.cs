using Uintra.Core.Helpers;

namespace Uintra.Bulletins
{
    public class BulletinsSection
    {
        protected const string Alias = "bulletins";
        protected const string Name = "Bulletins";
        protected const string Icon = "icon-notepad color-red";

        public static void AddSectionToAllUsers()
        {
            SectionHelper.AddSectionToAllUsers(Name, Alias, Icon);
        }
    }
}