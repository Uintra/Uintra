using System.Collections.Generic;
using System;
using Uintra.Core.ApplicationSettings.Models;

namespace Uintra.Core.ApplicationSettings
{
    public interface IApplicationSettings
    {
        string MailNotificationNoReplyEmail { get; }
        string MailNotificationNoReplyName { get; }
        int MonthlyEmailJobDay { get; }
        IEnumerable<string> VideoFileTypes { get; }
        string MemberApiAuthentificationEmail { get; }
        string UintraDocumentationLinkTemplate { get; }
        string DefaultToolbarConfig { get; }
		Guid QaKey { get; }
        GoogleOAuth GoogleOAuth { get; }
        bool UmbracoUseSSL { get; }
        IEnumerable<string> UintraSuperUsers { get; }
        bool DaytimeSavingOffset { get; }
    }
}
