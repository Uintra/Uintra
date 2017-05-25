using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Compent.uIntra.Core.ApplicationSettings
{
    public class uIntraApplicationSettings : ConfigurationSection, IuIntraApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string PinDaysRangeStartKey = "PinDaysRangeStart";
        private const string PinDaysRangeEndKey = "PinDaysRangeEnd";
        private const string NotWebMasterRoleDisabledDocumentTypesKey = "NotWebMasterRole.DisabledDocumentTypes";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];
        public int PinDaysRangeStart => int.Parse(ConfigurationManager.AppSettings[PinDaysRangeStartKey]);
        public int PinDaysRangeEnd => int.Parse(ConfigurationManager.AppSettings[PinDaysRangeEndKey]);

        public IEnumerable<string> NotWebMasterRoleDisabledDocumentTypes => ConfigurationManager.AppSettings[NotWebMasterRoleDisabledDocumentTypesKey]
            .Split(new[] { "," }, StringSplitOptions.None)
            .Select(el => el.Trim());
    }
}