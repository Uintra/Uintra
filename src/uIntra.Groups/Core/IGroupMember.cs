using System;
using System.Collections.Generic;
using Uintra.Core.User;

namespace Uintra.Groups
{
    public interface IGroupMember : IIntranetUser
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}