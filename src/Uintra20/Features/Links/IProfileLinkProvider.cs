using System;
using Uintra20.Core.Member;
using Uintra20.Core.Member.Abstractions;

namespace Uintra20.Features.Links
{
    public interface IProfileLinkProvider
    {
        string GetProfileLink(IIntranetMember member);
        string GetProfileLink(Guid userId);
    }
}
