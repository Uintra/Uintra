using System;
using System.Collections.Generic;

namespace Uintra20.Features.Groups
{
    public interface IGroupMember
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}
