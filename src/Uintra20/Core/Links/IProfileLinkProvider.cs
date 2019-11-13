using System;
using Uintra20.Core.User;

namespace Uintra20.Core.Links
{
    public interface IProfileLinkProvider
    {
        string GetProfileLink(IIntranetMember member);
        string GetProfileLink(Guid userId);
    }
}
