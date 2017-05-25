using Umbraco.Core;

namespace uIntra.Events.Dashboard
{
    public class EventsSection
    {
        protected const string Alias = "events";
        protected const string Name = "events";
        protected const string Icon = "icon-calendar-alt _events-section-color";

        public static void AddSectionToAllUsers(ApplicationContext applicationContext)
        {
            var section = applicationContext.Services.SectionService.GetByAlias(Alias);
            if (section != null) return;

            applicationContext.Services.SectionService.MakeNew(Name, Alias, Icon, 1);
            applicationContext.Services.UserService.AddSectionToAllUsers(Alias);
        }
    }
}