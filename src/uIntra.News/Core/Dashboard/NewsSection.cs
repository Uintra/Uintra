using Umbraco.Core;

namespace uIntra.News.Dashboard
{
    public class NewsSection
    {
        protected const string Alias = "news";
        protected const string Name = "News";
        protected const string Icon = "icon-newspaper-alt _news-section-color";

        public static void AddSectionToAllUsers(ApplicationContext applicationContext)
        {
            var section = applicationContext.Services.SectionService.GetByAlias(Alias);
            if (section != null) return;

            applicationContext.Services.SectionService.MakeNew(Name, Alias, Icon, 1);
            applicationContext.Services.UserService.AddSectionToAllUsers(Alias);
        }
    }
}