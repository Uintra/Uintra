using System;
using Uintra.Core.Member.Abstractions;
using Uintra.Features.Links.Models;

namespace Uintra.Features.Links
{
    public interface IProfileLinkProvider
    {
        UintraLinkModel GetProfileLink(IIntranetMember member);
        UintraLinkModel GetProfileLink(Guid userId);
    }
}
