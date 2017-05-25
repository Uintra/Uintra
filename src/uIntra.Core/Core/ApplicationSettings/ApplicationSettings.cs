using System.Configuration;

namespace uCommunity.Core.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string PinDaysRangeStartKey = "PinDaysRangeStart";
        private const string PinDaysRangeEndKey = "PinDaysRangeEnd";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];
        public int PinDaysRangeStart => int.Parse(ConfigurationManager.AppSettings[PinDaysRangeStartKey]);
        public int PinDaysRangeEnd => int.Parse(ConfigurationManager.AppSettings[PinDaysRangeEndKey]);
    }
}
