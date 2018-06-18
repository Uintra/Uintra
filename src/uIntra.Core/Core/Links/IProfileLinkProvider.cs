using System;
using Uintra.Core.User;

namespace Uintra.Core.Links
{
    public interface IProfileLinkProvider
    {
        string GetProfileLink(IIntranetUser user);
        string GetProfileLink(Guid userId);
    }
}
