using Compent.Shared.Extensions.Bcl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Uintra20.Infrastructure.ApplicationSettings.Models;
using static System.Configuration.ConfigurationManager;

namespace Uintra20.Infrastructure.ApplicationSettings
{
    public class ApplicationSettings : ConfigurationSection, IApplicationSettings
    {
        private GoogleOAuth _googleOAuth;

        private const string DefaultAvatarPathKey = "DefaultAvatarPath";
        private const string MonthlyEmailJobDayKey = "MonthlyEmailJobDay";
        private const string VideoFileTypesKey = "VideoFileTypes";
        private const string QaKeyKey = "QaKey";
        private const string MemberApiAuthenticationEmailKey = "MemberApiAuthentificationEmail";
        private const string GoogleClientIdKey = "Google.OAuth.ClientId";
        private const string GoogleDomainKey = "Google.OAuth.Domain";
        private const string GoogleEnabledKey = "Google.OAuth.Enabled";
        private const string UmbracoUseSSLKey = "umbracoUseSSL";
        private const string AdminSecretKey = "AdminControllerSecretKey";
        private const string UintraDocumentationLinkTemplateKey = "UintraDocumentationLinkTemplate";
        public const string MailNotificationNoReplyEmailKey = "Notifications.Mail.NoReplyEmail";
        public const string MailNotificationNoReplyNameKey = "Notifications.Mail.NoReplyName";
        public const string UintraSuperUsersKey = "UintraSuperUsers";
        public const string DaytimeSavingOffsetKey = "DaytimeSavingOffset";
        
        public string MailNotificationNoReplyEmail => AppSettings[MailNotificationNoReplyEmailKey];
        public string MailNotificationNoReplyName => AppSettings[MailNotificationNoReplyNameKey];

        public string AdminControllerSecretKey => AppSettings["AdminSecretKey"];

        public IEnumerable<string> VideoFileTypes => AppSettings[VideoFileTypesKey]
            .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(el => el.Trim());

        public bool DaytimeSavingOffset => bool.Parse(AppSettings[DaytimeSavingOffsetKey]);
        public Guid QaKey => Guid.Parse(AppSettings[QaKeyKey]);

        public int MonthlyEmailJobDay => Convert.ToInt32(AppSettings[MonthlyEmailJobDayKey]);

        public string MemberApiAuthenticationEmail => AppSettings[MemberApiAuthenticationEmailKey];

        public string UintraDocumentationLinkTemplate => AppSettings[UintraDocumentationLinkTemplateKey];

        public GoogleOAuth GoogleOAuth
        {
            get
            {
                if (_googleOAuth != null)
                    return _googleOAuth;

                _googleOAuth = new GoogleOAuth
                {
                    ClientId = string.Empty
                };

                if (!bool.TryParse(AppSettings[GoogleEnabledKey], out var enabled) || !enabled) 
                    return _googleOAuth;

                var clientId = AppSettings[GoogleClientIdKey];

                if (!clientId.HasValue())
                    throw new ArgumentNullException("Google client id");

                _googleOAuth.ClientId = clientId;
                _googleOAuth.Domain = AppSettings[GoogleDomainKey];
                _googleOAuth.Enabled = true;

                return _googleOAuth;
            }
        }

        public bool UmbracoUseSsl => bool.TryParse(AppSettings[UmbracoUseSSLKey], out var enabled) && enabled;

        public IEnumerable<string> UintraSuperUsers
        {
            get
            {
                var value = AppSettings[UintraSuperUsersKey];
                return string.IsNullOrEmpty(value) ? Enumerable.Empty<string>() : value.Split(',');
            }
        }
    }
}
