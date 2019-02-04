using System;
using Uintra.Core.User;

namespace Uintra.Core.Links
{
    public interface IProfileLinkProvider
    {
        string GetProfileLink(IIntranetMember member);
        string GetProfileLink(Guid userId);
    }
}
