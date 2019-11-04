using System.Collections.Generic;
using System;
using Uintra20.Core.ApplicationSettings.Models;

namespace Uintra20.Core.ApplicationSettings
{
    public interface IApplicationSettings
    {
        string MailNotificationNoReplyEmail { get; }
        string MailNotificationNoReplyName { get; }
        int MonthlyEmailJobDay { get; }
        IEnumerable<string> VideoFileTypes { get; }
        string MemberApiAuthentificationEmail { get; }
        string UintraDocumentationLinkTemplate { get; }
        Guid QaKey { get; }
        GoogleOAuth GoogleOAuth { get; }
        bool UmbracoUseSSL { get; }
        IEnumerable<string> UintraSuperUsers { get; }
        bool DaytimeSavingOffset { get; }
    }
}
