using System;
using Uintra20.Core.Member;

namespace Uintra20.Features.Links
{
    public interface IProfileLinkProvider
    {
        string GetProfileLink(IIntranetMember member);
        string GetProfileLink(Guid userId);
    }
}
