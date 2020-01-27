using System;
using Uintra20.Core.Member.Abstractions;
using Uintra20.Features.Links.Models;

namespace Uintra20.Features.Links
{
    public interface IProfileLinkProvider
    {
        UintraLinkModel GetProfileLink(IIntranetMember member);
        UintraLinkModel GetProfileLink(Guid userId);
    }
}
