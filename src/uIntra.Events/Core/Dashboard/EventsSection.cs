using Uintra.Core.Helpers;

namespace Uintra.Events.Dashboard
{
    public class EventsSection
    {
        protected const string Alias = "events";
        protected const string Name = "events";
        protected const string Icon = "icon-calendar-alt color-red";

        public static void AddSectionToAllUsers()
        {
            SectionHelper.AddSectionToAllUsers(Name, Alias, Icon);
        }           
    }
}