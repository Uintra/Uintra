using System;
using System.Collections.Generic;

namespace Uintra.Groups
{
    public interface IGroupable
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}
