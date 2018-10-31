using System.Collections.Generic;
using System;

namespace Uintra.Core.ApplicationSettings
{
    public interface IApplicationSettings
    {
        string DefaultAvatarPath { get; }
        int MonthlyEmailJobDay { get; }
        IEnumerable<string> VideoFileTypes { get; }
        string MemberApiAuthentificationEmail { get; }
        Guid QaKey { get; }
    }
}
