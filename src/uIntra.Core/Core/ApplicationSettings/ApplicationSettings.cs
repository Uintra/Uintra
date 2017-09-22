using System;
using System.Configuration;

namespace uIntra.Core.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string MonthlyEmailJobDayKey = "MonthlyEmailJobDay";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];

        public int MonthlyEmailJobDay => Convert.ToInt32(ConfigurationManager.AppSettings[MonthlyEmailJobDayKey]);
    }
}
