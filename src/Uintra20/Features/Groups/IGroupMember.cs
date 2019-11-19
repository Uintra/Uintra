using System;
using System.Collections.Generic;
using Uintra20.Features.User;

namespace Uintra20.Features.Groups
{
    public interface IGroupMember : IIntranetMember
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}
