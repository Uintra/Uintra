using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Uintra.Core.ApplicationSettings.Models;

namespace Uintra.Core.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private GoogleOAuth _googleOAuth;

        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string MonthlyEmailJobDayKey = "MonthlyEmailJobDay";
        private const string VideoFileTypesKey = "VideoFileTypes";
        private const string QaKeyKey = "QaKey";
        private const string MemberApiAuthentificationEmailKey = "MemberApiAuthentificationEmail";
        private const string GoogleClientIdKey = "Google.OAuth.ClientId";
        private const string GoogleDomainKey = "Google.OAuth.Domain";
        private const string GoogleEnabledKey = "Google.OAuth.Enabled";

        public string DefaultAvatarPath => ConfigurationManager.AppSettings[DefaultAvatarPathKey];

        public IEnumerable<string> VideoFileTypes => ConfigurationManager.AppSettings[VideoFileTypesKey]
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(el => el.Trim());

        public Guid QaKey => Guid.Parse(ConfigurationManager.AppSettings[QaKeyKey]);

        public int MonthlyEmailJobDay => Convert.ToInt32(ConfigurationManager.AppSettings[MonthlyEmailJobDayKey]);

        public string MemberApiAuthentificationEmail => ConfigurationManager.AppSettings[MemberApiAuthentificationEmailKey];

        public GoogleOAuth GoogleOAuth
        {
            get
            {
                if (_googleOAuth != null)
                    return _googleOAuth;
                _googleOAuth = new GoogleOAuth() { ClientId = string.Empty };
                if (bool.TryParse(ConfigurationManager.AppSettings[GoogleEnabledKey], out var enabled) && enabled)
                {
                    var clientId = ConfigurationManager.AppSettings[GoogleClientIdKey];
                    if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentNullException("Google client id");
                    _googleOAuth.ClientId = clientId;
                    _googleOAuth.Domain = ConfigurationManager.AppSettings[GoogleDomainKey];
                    _googleOAuth.Enabled = enabled;
                }
                return _googleOAuth;
            }
        }
    }
}
