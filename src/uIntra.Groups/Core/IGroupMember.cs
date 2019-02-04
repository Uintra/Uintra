using System;
using System.Collections.Generic;
using Uintra.Core.User;

namespace Uintra.Groups
{
    public interface IGroupMember : IIntranetMember
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}