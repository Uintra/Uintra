using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Compent.uCommunity.Core.ApplicationSettings
{
    public class UcommunityApplicationSettings : ConfigurationSection, IUcommunityApplicationSettings
    {
        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string NotWebMasterRoleDisabledDocumentTypesKey = "NotWebMasterRole.DisabledDocumentTypes";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];

        public IEnumerable<string> NotWebMasterRoleDisabledDocumentTypes => ConfigurationManager.AppSettings[NotWebMasterRoleDisabledDocumentTypesKey]
            .Split(new[] { "," }, StringSplitOptions.None)
            .Select(el => el.Trim());
    }
}