using System;
using System.Collections.Generic;
using Uintra20.Infrastructure.ApplicationSettings.Models;

namespace Uintra20.Infrastructure.ApplicationSettings
{
    public interface IApplicationSettings
    {
        bool DaytimeSavingOffset { get; }
        bool UmbracoUseSsl { get; }
        int MonthlyEmailJobDay { get; }
        Guid QaKey { get; }
        string MailNotificationNoReplyEmail { get; }
        string MailNotificationNoReplyName { get; }
        string MemberApiAuthenticationEmail { get; }
        string UintraDocumentationLinkTemplate { get; }
        string AdminControllerSecretKey { get; }
        IEnumerable<string> VideoFileTypes { get; }
        GoogleOAuth GoogleOAuth { get; }
        IEnumerable<string> UintraSuperUsers { get; }
    }
}
