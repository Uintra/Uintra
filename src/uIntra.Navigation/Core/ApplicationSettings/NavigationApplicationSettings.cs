using System.Configuration;

namespace uIntra.Navigation
{
    public class NavigationApplicationSettings : ConfigurationSection, INavigationApplicationSettings
    {
        private const string MyLinksBulletinsTitleLengthKey = "MyLinks.BulletinsTitleLength";

        public int MyLinksBulletinsTitleLength => int.Parse(ConfigurationManager.AppSettings[MyLinksBulletinsTitleLengthKey]);
    }
}
