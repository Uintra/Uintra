using System.Configuration;

namespace Uintra.Navigation
{
    public class NavigationApplicationSettings : ConfigurationSection, INavigationApplicationSettings
    {
        private const string MyLinksBulletinsTitleLengthKey = "MyLinks.BulletinsTitleLength";

        public int MyLinksBulletinsTitleLength => int.Parse(ConfigurationManager.AppSettings[MyLinksBulletinsTitleLengthKey]);
    }
}
