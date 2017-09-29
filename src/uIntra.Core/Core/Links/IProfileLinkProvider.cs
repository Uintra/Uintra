using System;
using uIntra.Core.User;

namespace uIntra.Core.Links
{
    public interface IProfileLinkProvider
    {
        string GetProfileLink(IIntranetUser user);
        string GetProfileLink(Guid userId);
    }
}
