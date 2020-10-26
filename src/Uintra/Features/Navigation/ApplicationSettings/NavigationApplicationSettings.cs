using System.Configuration;

namespace Uintra.Features.Navigation.ApplicationSettings
{
    public class NavigationApplicationSettings : ConfigurationSection, INavigationApplicationSettings
    {
        private const string MyLinksActivityTitleLengthKey = "MyLinks.ActivityTitleLength";

        public int MyLinksActivityTitleLength => int.Parse(ConfigurationManager.AppSettings[MyLinksActivityTitleLengthKey]);
    }
}