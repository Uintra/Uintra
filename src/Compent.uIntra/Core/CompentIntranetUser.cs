using System;
using System.Collections.Generic;
using System.Linq;
using uIntra.Groups;
using uIntra.Users;

namespace Compent.uIntra.Core
{
    public class CompentIntranetUser : IntranetUser, IGroupMember
    {
        public IEnumerable<Guid> GroupIds { get; set; } = Enumerable.Empty<Guid>();
    }
}