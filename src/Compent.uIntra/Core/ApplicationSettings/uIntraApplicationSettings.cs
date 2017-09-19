using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Compent.uIntra.Core.ApplicationSettings
{
    public class UintraApplicationSettings : ConfigurationSection, IuIntraApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string PinDaysRangeStartKey = "PinDaysRangeStart";
        private const string PinDaysRangeEndKey = "PinDaysRangeEnd";
        private const string NotWebMasterRoleDisabledDocumentTypesKey = "NotWebMasterRole.DisabledDocumentTypes";
        private const string MonthlyEmailJobDayKey = "MonthlyEmailJobDay";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];
        public int PinDaysRangeStart => int.Parse(ConfigurationManager.AppSettings[PinDaysRangeStartKey]);
        public int PinDaysRangeEnd => int.Parse(ConfigurationManager.AppSettings[PinDaysRangeEndKey]);
        public int MonthlyEmailJobDay => int.Parse(ConfigurationManager.AppSettings[MonthlyEmailJobDayKey]);

        public IEnumerable<string> NotWebMasterRoleDisabledDocumentTypes => ConfigurationManager.AppSettings[NotWebMasterRoleDisabledDocumentTypesKey]
            .Split(new[] { "," }, StringSplitOptions.None)
            .Select(el => el.Trim());
    }
}