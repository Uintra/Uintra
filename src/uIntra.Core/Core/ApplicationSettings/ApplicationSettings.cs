using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace uIntra.Core.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string MonthlyEmailJobDayKey = "MonthlyEmailJobDay";
        private const string VideoFileTypesKey = "VideoFileTypes";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];

        public int MonthlyEmailJobDay => Convert.ToInt32(ConfigurationManager.AppSettings[MonthlyEmailJobDayKey]);

        public IEnumerable<string> VideoFileTypes => ConfigurationManager.AppSettings[VideoFileTypesKey]
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(el => el.Trim());
    }
}
