using System;
using System.Collections.Generic;

namespace uIntra.Groups
{
    public interface IGroupMember
    {
        Guid Id { get; }

        IEnumerable<Guid> GroupIds { get; set; }
    }
}