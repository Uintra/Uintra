using System;
using System.Collections.Generic;
using Uintra20.Core.User;

namespace Uintra20.Core.Groups
{
    public interface IGroupMember : IIntranetMember
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}
