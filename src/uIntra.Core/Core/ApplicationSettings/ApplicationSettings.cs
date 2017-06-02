using System.Configuration;

namespace uIntra.Core.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];
    }
}
