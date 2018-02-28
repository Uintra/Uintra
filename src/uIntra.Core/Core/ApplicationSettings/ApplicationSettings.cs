using System;
using System.Configuration;

namespace Uintra.Core.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string MonthlyEmailJobDayKey = "MonthlyEmailJobDay";
        private const string QaKeyKey = "QaKey";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];

        public Guid QaKey => Guid.Parse(ConfigurationManager.AppSettings[QaKeyKey]);

        public int MonthlyEmailJobDay => Convert.ToInt32(ConfigurationManager.AppSettings[MonthlyEmailJobDayKey]);
    }
}
