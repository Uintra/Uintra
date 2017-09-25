using System;
using System.Collections.Generic;
using uIntra.Core.User;

namespace uIntra.Groups
{
    public interface IGroupMember : IIntranetUser
    {
        IEnumerable<Guid> GroupIds { get; set; }
    }
}