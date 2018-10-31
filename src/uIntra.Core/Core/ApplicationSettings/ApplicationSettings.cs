using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Uintra.Core.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string MonthlyEmailJobDayKey = "MonthlyEmailJobDay";
        private const string VideoFileTypesKey = "VideoFileTypes";
        private const string QaKeyKey = "QaKey";
        private const string MemberApiAuthentificationEmailKey = "MemberApiAuthentificationEmail";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];

        public IEnumerable<string> VideoFileTypes => ConfigurationManager.AppSettings[VideoFileTypesKey]
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(el => el.Trim());

        public Guid QaKey => Guid.Parse(ConfigurationManager.AppSettings[QaKeyKey]);

        public int MonthlyEmailJobDay => Convert.ToInt32(ConfigurationManager.AppSettings[MonthlyEmailJobDayKey]);

        public string MemberApiAuthentificationEmail => ConfigurationManager.AppSettings[MemberApiAuthentificationEmailKey];
    }
}
