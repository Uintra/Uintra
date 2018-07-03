using Uintra.Core.Helpers;

namespace Uintra.News.Dashboard
{
    public class NewsSection
    {
        protected const string Alias = "news";
        protected const string Name = "News";
        protected const string Icon = "icon-newspaper-alt color-red";

        public static void AddSectionToAllUsers()
        {
            SectionHelper.AddSectionToAllUsers(Name, Alias, Icon);
        }
    }
}