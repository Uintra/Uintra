using System.Configuration;

namespace Uintra20.Features.Navigation.ApplicationSettings
{
    public class NavigationApplicationSettings : ConfigurationSection, INavigationApplicationSettings
    {
        private const string MyLinksBulletinsTitleLengthKey = "MyLinks.BulletinsTitleLength";

        public int MyLinksBulletinsTitleLength => int.Parse(ConfigurationManager.AppSettings[MyLinksBulletinsTitleLengthKey]);
    }
}