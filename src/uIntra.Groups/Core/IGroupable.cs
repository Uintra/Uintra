using System;
using System.Collections.Generic;

namespace uIntra.Groups
{
    public interface IGroupable
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}
