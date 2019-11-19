using System;
using Uintra20.Features.User;

namespace Uintra20.Features.Links
{
    public interface IProfileLinkProvider
    {
        string GetProfileLink(IIntranetMember member);
        string GetProfileLink(Guid userId);
    }
}
