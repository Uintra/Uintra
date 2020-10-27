using System;
using System.Collections.Generic;
using Uintra.Core.Member.Abstractions;

namespace Uintra.Features.Groups
{
    public interface IGroupMember : IIntranetMember
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}
